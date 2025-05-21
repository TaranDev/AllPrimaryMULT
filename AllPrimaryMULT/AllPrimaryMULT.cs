using BepInEx;
using BepInEx.Configuration;
using R2API;
using RoR2;
using RiskOfOptions;
using RiskOfOptions.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using EntityStates;
using RoR2.Audio;
using RoR2.Orbs;
using ExtraSkillSlots;
using UnityEngine.AddressableAssets;
using RoR2.Skills;
using static RoR2.Skills.SkillFamily;
using System;
using EntityStates.Toolbot;
using IL.RoR2.Mecanim;

namespace AllPrimaryMULT
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class AllPrimaryMULT : BaseUnityPlugin
    {

        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "TaranDev";
        public const string PluginName = "AllPrimaryMULT";
        public const string PluginVersion = "1.0.0";

        public void Awake()
        {
            Log.Init(Logger);

            configs();

            GameObject multBodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/ToolbotBody.prefab").WaitForCompletion();

            SkillCreator c = new SkillCreator();

            SkillDef skillDef = c.AllPrimarySkillDef();

            ContentAddition.AddSkillDef(skillDef);

            SkillFamily skillFamily = multBodyPrefab.GetComponent<SkillLocator>().primary.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant()
            {
                skillDef = skillDef,
                //viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false)
            };

            SkillFamily skillFamily2 = Addressables.LoadAssetAsync<SkillFamily>("RoR2/Base/Toolbot/ToolbotBodyPrimary2.asset").WaitForCompletion();

            //SkillFamily skillFamily2 = multBodyPrefab.GetComponent<SkillLocator>().FindSkillByFamilyName("ToolbotBodyPrimary2").skillFamily;
            Array.Resize(ref skillFamily2.variants, skillFamily2.variants.Length + 1);
            skillFamily2.variants[skillFamily2.variants.Length - 1] = new SkillFamily.Variant()
            {
                skillDef = skillDef,
                //viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false)
            };
        }

        private void OnEnable()
        {
            On.RoR2.Util.PlaySound_string_GameObject += PlaySound;
            On.RoR2.Util.PlaySound_string_GameObject_string_float += PlaySound2;
            On.RoR2.Audio.LoopSoundManager.PlaySoundLoopLocal += PlaySoundLoopLocal;
            On.RoR2.Audio.PointSoundManager.EmitSoundLocal += EmitSoundLocal;
        }

        private uint EmitSoundLocal(On.RoR2.Audio.PointSoundManager.orig_EmitSoundLocal orig, AkEventIdArg akEventId, Vector3 position)
        {
            if (!scrapSound.Value && akEventId.id == 2695190594)
            {
                return 0u;
            }
            return orig(akEventId, position);
        }

        private RoR2.Audio.LoopSoundManager.SoundLoopPtr PlaySoundLoopLocal(On.RoR2.Audio.LoopSoundManager.orig_PlaySoundLoopLocal orig, GameObject gameObject, RoR2.Audio.LoopSoundDef loopSoundDef)
        {

            if(loopSoundDef && loopSoundDef.startSoundName != null)
            {
                if (!nailgunSound.Value && loopSoundDef.startSoundName.Contains("nailgun") ||
                   !rebarSound.Value && loopSoundDef.startSoundName.Contains("rebar") ||
                   !scrapSound.Value && loopSoundDef.startSoundName.Contains("scrap") ||
                   !sawSound.Value && loopSoundDef.startSoundName.Contains("buzzsaw"))
                {
                    return new LoopSoundManager.SoundLoopPtr(LoopSoundManager.soundLoopHeap.Alloc());
                }
            }

            return orig(gameObject, loopSoundDef);

        }

        private uint PlaySound(On.RoR2.Util.orig_PlaySound_string_GameObject orig, string soundString, GameObject gameObject)
        {
            if (soundString != null)
            {
                if (!nailgunSound.Value && (soundString.Contains("Play_MULT_m1_smg_shoot") || soundString.Contains("Play_item_goldgat_winddown")) ||
                   !rebarSound.Value && soundString.Contains("Play_MULT_m1_snipe_shoot") ||
                   !scrapSound.Value && soundString.Contains("Play_MULT_m1_grenade_launcher_shoot") ||
                   !sawSound.Value && (soundString.Contains("Play_MULT_m1_sawblade_start") || soundString.Contains("Play_MULT_m1_sawblade_stop")))
                {
                    return 0u;
                }
            }
            return orig(soundString, gameObject);
        }

        private uint PlaySound2(On.RoR2.Util.orig_PlaySound_string_GameObject_string_float orig, string soundString, GameObject gameObject, string RTPCstring, float RTPCvalue)
        {
            if (soundString != null)
            {
                if (!nailgunSound.Value && (soundString.Contains("Play_MULT_m1_smg_shoot") || soundString.Contains("Play_item_goldgat_winddown")) ||
                   !rebarSound.Value && soundString.Contains("Play_MULT_m1_snipe_shoot") ||
                   !scrapSound.Value && soundString.Contains("Play_MULT_m1_grenade_launcher_shoot") ||
                   !sawSound.Value && (soundString.Contains("Play_MULT_m1_sawblade_start") || soundString.Contains("Play_MULT_m1_sawblade_stop")))
                {
                    return 0u;
                }
            }
            return orig(soundString, gameObject, RTPCstring, RTPCvalue);
        }

        public static ConfigEntry<bool> nailgunSound;

        public static ConfigEntry<bool> rebarSound;

        public static ConfigEntry<bool> scrapSound;

        public static ConfigEntry<bool> sawSound;

        //public static ConfigEntry<bool> accurateNailgun;

        private void configs()
        {

            nailgunSound = Config.Bind("General", "Play Auto-Nailgun Sound Effects", true, "If Auto-Nailgun sound effects should play.\nDefault is true.");
            ModSettingsManager.AddOption(new CheckBoxOption(nailgunSound));

            rebarSound = Config.Bind("General", "Play Rebar-Puncher Sound Effects", true, "If Rebar-Puncher sound effects should play.\nDefault is true.");
            ModSettingsManager.AddOption(new CheckBoxOption(rebarSound));

            scrapSound = Config.Bind("General", "Play Scrap Launcher Sound Effects", true, "If Scrap Launcher sound effects should play. This includes the scrap explosion sounds.\nDefault is true.");
            ModSettingsManager.AddOption(new CheckBoxOption(scrapSound));

            sawSound = Config.Bind("General", "Play Power-Saw Sound Effects", true, "If Power-Saw sound effects should play.\nDefault is true.");
            ModSettingsManager.AddOption(new CheckBoxOption(sawSound));

            /*accurateNailgun = Config.Bind("General", "Accurate Nailgun", false, "Keep the base game bug causing Auto-Nailgun to be accurate while using Power-Saw\nDefault is false.");
            ModSettingsManager.AddOption(new CheckBoxOption(accurateNailgun));*/
        }

    }
}