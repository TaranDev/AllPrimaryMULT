using BepInEx;
using EntityStates;
using EntityStates.Toolbot;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem.PlaybackState;
using UnityEditor;
using IL.RoR2.UI;

namespace AllPrimaryMULT
{
    [BepInDependency(R2API.ContentManagement.R2APIContentManager.PluginGUID)]
    public class SkillCreator : MonoBehaviour
    {



        public SkillDef AllPrimarySkillDef()
        {
            GameObject multBodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/ToolbotBody.prefab").WaitForCompletion();
            
            GameObject crosshair = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/SMGCrosshair.prefab").WaitForCompletion();

            Texture2D skillIcons = Addressables.LoadAssetAsync<Texture2D>("RoR2/Base/Toolbot/texToolbotSkillIcons.png").WaitForCompletion();
            Sprite baseSkillIcon = Sprite.Create(skillIcons, new Rect(0f, 256f, 512, 512), new Vector2(0.5f, 0.5f), 50);

            // Shame this didn't work
            //Sprite allSkillIcon = GenerateAllSkillSprite();

            /*Sprite[] sprites = Resources.LoadAll<Sprite>(skillIcon.name);*/

            /*Log.Info(skillIcons.name);
            Log.Info(skillIcons.width);
            Log.Info(skillIcons.height);*/

            // Now we must create a SkillDef
            ToolbotWeaponSkillDef mySkillDef = ScriptableObject.CreateInstance<ToolbotWeaponSkillDef>();

            //Check step 2 for the code of the CustomSkillsTutorial.MyEntityStates.SimpleBulletAttack class
            float cooldown = 0f;

            mySkillDef.activationState = new SerializableEntityStateType(typeof(AllPrimaryAttack));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = cooldown;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = true;
            mySkillDef.cancelSprintingOnActivation = true;
            mySkillDef.fullRestockOnAssign = false;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            // For the skill icon, you will have to load a Sprite from your own AssetBundle
            mySkillDef.icon = baseSkillIcon;
            mySkillDef.skillDescriptionToken = "Rapidly fire nails for <style=cIsDamage>70% damage</style>, a piercing rebar that deals <style=cIsDamage>600% damage</style>, a rocket that explodes for <style=cIsDamage>360% damage</style>, and saw nearby enemies for <style=cIsDamage>1000% damage per second</style>. Hold up to 4 rockets, finish shooting with a blast of <style=cIsDamage>12</style> nails.";
            mySkillDef.skillName = "Auto-Rebar-Saw-Launcher";
            mySkillDef.skillNameToken = "Auto-Rebar-Saw-Launcher";
            mySkillDef.crosshairPrefab = crosshair;
            mySkillDef.crosshairSpreadCurve = AnimationCurve.Linear(0, 2, 1, 5); //.GetSkillDef(SkillCatalog.FindSkillIndexByName("FireNailgun")).GetFieldValue<AnimationCurve>("Crosshair Spread Curve");
            // This adds our skilldef. If you don't do this, the skill will not work.
            try
            {
                ContentAddition.AddSkillDef(mySkillDef);
            }
            catch (Exception e)
            {

            }


            return mySkillDef;
        }

        public Sprite GenerateAllSkillSprite()
        {
            //RoR2/Base/Common/texEquipmentBGIcon.png
            Texture2D skillIcons = Addressables.LoadAssetAsync<Texture2D>("RoR2/Base/Toolbot/texToolbotSkillIcons.png").WaitForCompletion();

            Texture2D myTexture2D = new Texture2D(skillIcons.width, skillIcons.height, skillIcons.format, skillIcons.mipmapCount > 1);

            Graphics.CopyTexture(skillIcons, myTexture2D);

            myTexture2D.Apply();

            /*// Create a temporary RenderTexture of the same size as the texture
            RenderTexture tmp = RenderTexture.GetTemporary(
                                skillIcons.width,
                                skillIcons.height,
                                0,
                                RenderTextureFormat.Default,
                                RenderTextureReadWrite.Linear);


            // Blit the pixels on texture to the RenderTexture
            Graphics.Blit(skillIcons, tmp);


            // Backup the currently set RenderTexture
            RenderTexture previous = RenderTexture.active;


            // Set the current RenderTexture to the temporary one we created
            RenderTexture.active = tmp;


            // Create a new readable Texture2D to copy the pixels to it
            Texture2D myTexture2D = new Texture2D(skillIcons.width, skillIcons.height);


            // Copy the pixels from the RenderTexture to the new Texture
            myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            myTexture2D.Apply();


            // Reset the active RenderTexture
            RenderTexture.active = previous;


            // Release the temporary RenderTexture
            RenderTexture.ReleaseTemporary(tmp);*/


            // "myTexture2D" now has the same pixels from "texture" and it's re


            Sprite sawSkillIcon = Sprite.Create(myTexture2D, new Rect(768f, 0f, 256, 256), new Vector2(0.5f, 0.5f), 50);
            Sprite baseSkillIcon = Sprite.Create(myTexture2D, new Rect(0f, 256f, 512, 512), new Vector2(0.5f, 0.5f), 50);
            return sawSkillIcon;
            var newTex = new Texture2D(512, 512);
            
            for (int x = 0; x < baseSkillIcon.texture.width; x++)
            {
                for (int y = 0; y < baseSkillIcon.texture.height; y++)
                {
                    Color colour;
                    if (x >= 256 && y < 256)
                    {
                        colour = sawSkillIcon.texture.GetPixel(x, y);
                    }
                    else
                    {
                        colour = baseSkillIcon.texture.GetPixel(x, y);
                    }
                    newTex.SetPixel(x, y, colour);
                }
            }

            newTex.Apply();

            var allSkillIcon = Sprite.Create(newTex, new Rect(0, 0, newTex.width, newTex.height), new Vector2(0.5f, 0.5f));

            return allSkillIcon;
        }
    }

    public class AllPrimaryAttack : EntityStates.Toolbot.FireNailgun
    {
        private static GameObject buzzsawEffectLoop = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/ToolbotBuzzsawEffectLoop.prefab").WaitForCompletion();

        private static GameObject buzzsawImpactEffectLoo = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/ToolbotBuzzsawImpactEffectLoop.prefab").WaitForCompletion();
        private static ToolbotWeaponSkillDef nailgun = Addressables.LoadAssetAsync<ToolbotWeaponSkillDef>("RoR2/Base/Toolbot/ToolbotBodyFireNailgun.asset").WaitForCompletion();
        private static GameObject muzzleflashSmokeRing = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/MuzzleflashSmokeRing.prefab").WaitForCompletion();
        private static GameObject tracerToolbotRebar = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/TracerToolbotRebar.prefab").WaitForCompletion();
        private static GameObject impactSpear = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/ImpactSpear.prefab").WaitForCompletion();
        private static GameObject scrapProjectile = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/ToolbotGrenadeLauncherProjectile.prefab").WaitForCompletion();

        // Saw

        public static float damageCoefficientPerSecond = 10f;

        public static float procCoefficientPerSecond = 1f;

        public static string sawFireSoundString;

        public static string impactSoundString;

        public static string spinUpSoundString = "Play_MULT_m1_sawblade_start";

        public static string spinDownSoundString = "Play_MULT_m1_sawblade_stop";

        public static float sawSpreadBloomValue = 0.2f;

        public static float baseFireFrequency = 10f;

        public static GameObject spinEffectPrefab = buzzsawEffectLoop;

        public static GameObject spinImpactEffectPrefab = buzzsawImpactEffectLoo;

        public static GameObject impactEffectPrefab;

        public static float selfForceMagnitude = 600f;

        private OverlapAttack attack;

        private float fireFrequency;

        private float fireAge = 0f;

        private GameObject spinEffectInstance;

        private GameObject spinImpactEffectInstance;

        private bool hitOverlapLastTick;

        protected EffectManagerHelper _emh_spinEffectInstance;

        protected EffectManagerHelper _emh_spinImpactEffectInstance;

        private static int SpinBuzzsawStateHash = Animator.StringToHash("SpinBuzzsaw");

        private static int EnterBuzzsawStateHash = Animator.StringToHash("EnterBuzzsaw");

        private static int EmptyStateHash = Animator.StringToHash("Empty");

        private static int ExitBuzzsawStateHash = Animator.StringToHash("ExitBuzzsaw");

        private static int ImpactBuzzsawStateHash = Animator.StringToHash("ImpactBuzzsaw");

        public override string baseMuzzleName => "MuzzleBuzzsaw";

        GameObject sawBase = GameObject.Find("saw.base");

        //

        // Spear

        public float charge;

        public static float recoilAmplitude;

        private static int FireSpearStateHash = Animator.StringToHash("FireSpear");

        private static int FireSpearParamHash = Animator.StringToHash("FireSpear.playbackRate");

        /*Transform IToolbotPrimarySkillState.muzzleTransform { get; set; }

        string IToolbotPrimarySkillState.baseMuzzleName => muzzleName;

        bool IToolbotPrimarySkillState.isInDualWield { get; set; }

        int IToolbotPrimarySkillState.currentHand { get; set; }

        string IToolbotPrimarySkillState.muzzleName { get; set; }*/

        public SkillDef skillDef { get; set; }

        public GenericSkill activatorSkillSlot { get; set; }

        private float spearDuration;

        public static float spearBaseDuration = 1.7f;

        float spearCooldown;

        public string spearFireSoundString = "Play_MULT_m1_snipe_shoot";

        public GameObject spearMuzzleFlashPrefab = muzzleflashSmokeRing;

        public GameObject spearTracerEffectPrefab = tracerToolbotRebar;

        public GameObject spearHitEffectPrefab = impactSpear;

        public string spearMuzzleName = "MuzzleSpear";

        public int spearBulletCount = 1;

        public float spearBulletRadius = 1f;

        public float spearMinSpread = 0;

        public float spearMaxSpread = 0;

        public bool spearUseSmartCollision = true;

        public float spearDamageCoefficient = 6f;

        public float spearMaxDistance = 4000f;

        public float spearProcCoefficient = 1f;

        public float spearSpreadPitchScale = 0f;

        public float spearSpreadYawScale = 0f;

        //

        // Scrap

        string handBaseMuzzleName;

        public float scrapDamageCoefficient = 3.6f;

        public float scrapForce = 700f;

        public float scrapMinSpread = 0f;

        public float scrapMaxSpread = 1f;

        public float scrapBaseDuration = 0.3f;

        public float scrapBulletCount = 1;

        public float scrapProjectilePitchBonus = 0;

        public float scrapDelayBeforeFiringProjectile = 0;

        public float scrapBaseDelayBeforeFiringProjectile = 0.3f;

        public string scrapFireSoundString = "Play_MULT_m1_grenade_launcher_shoot";

        ScrapCooldownTracker scrapCooldownTracker;


        //


        //private float duration = 1f;

        //OnEnter() runs once at the start of the skill
        //All we do here is create a BulletAttack and fire it
        public override void OnEnter()
        {

            currentHand = 0;
            isInDualWield = EntityStateMachine.FindByCustomName(gameObject, "Body").state is ToolbotDualWield;
            //muzzleName = baseMuzzleName;
            handBaseMuzzleName = baseMuzzleName;
            //skillDef = activatorSkillSlot.skillDef;
            if (isInDualWield)
            {
                if ((object)activatorSkillSlot == skillLocator.primary)
                {
                    currentHand = -1;
                    handBaseMuzzleName = "DualWieldMuzzleL";
                }
                else if ((object)activatorSkillSlot == skillLocator.secondary)
                {
                    currentHand = 1;
                    handBaseMuzzleName = "DualWieldMuzzleR";
                }
            }
            if (muzzleName != null)
            {
                //muzzleTransform = GetModelChildLocator().FindChild(muzzleName);
            }

            //BaseToolbotPrimarySkillStateMethods.OnEnter(this, base.gameObject, base.skillLocator, GetModelChildLocator());
            base.OnEnter();
            
            // Saw

            fireFrequency = baseFireFrequency * attackSpeedStat;
            Transform modelTransform = GetModelTransform();

            Util.PlaySound(spinUpSoundString, base.gameObject);
            Util.PlaySound(sawFireSoundString, base.gameObject);

            if (!base.isInDualWield)
            {
                PlayAnimation("Gesture, Additive Gun", SpinBuzzsawStateHash);
                PlayAnimation("Gesture, Additive", EnterBuzzsawStateHash);
            }
            attack = new OverlapAttack();
            attack.attacker = base.gameObject;
            attack.inflictor = base.gameObject;
            attack.teamIndex = TeamComponent.GetObjectTeam(attack.attacker);
            attack.damage = damageCoefficientPerSecond * damageStat / baseFireFrequency;
            attack.procCoefficient = procCoefficientPerSecond / baseFireFrequency;
            attack.damageType.damageSource = DamageSource.Primary;
            if ((bool)impactEffectPrefab)
            {
                attack.hitEffectPrefab = impactEffectPrefab;
            }

            /*GameObject sawBase = GameObject.Find("saw.base");
            Log.Info(sawBase);
            Log.Info(sawBase.name);
            Log.Info(sawBase.transform.localScale);
            sawBase.transform.localScale = new Vector3(100, 100, 100);
            Log.Info(sawBase.transform.localScale);*/

            if ((bool)modelTransform)
            {
                string groupName = "Buzzsaw";
                if (base.isInDualWield)
                {
                    if (base.currentHand == -1)
                    {
                        groupName = "BuzzsawL";
                    }
                    else if (base.currentHand == 1)
                    {
                        groupName = "BuzzsawR";
                    }
                }
                attack.hitBoxGroup = HitBoxGroup.FindByGroupName(modelTransform.gameObject, groupName);
/*                foreach (HitBox hitBox in attack.hitBoxGroup.hitBoxes) {
                    hitBox.gameObject.transform.localScale = Vector3.one * 100;
                }*/
                
            }
            if ((bool)base.muzzleTransform)
            {
                if ((bool)spinEffectPrefab)
                {
                    if (!EffectManager.ShouldUsePooledEffect(spinEffectPrefab))
                    {
                        spinEffectInstance = UnityEngine.Object.Instantiate(spinEffectPrefab, base.isInDualWield ? base.muzzleTransform.position : base.muzzleTransform.position + base.muzzleTransform.up, base.muzzleTransform.rotation);
                    }
                    else
                    {
                        _emh_spinEffectInstance = EffectManager.GetAndActivatePooledEffect(spinEffectPrefab, base.isInDualWield ? base.muzzleTransform.position : base.muzzleTransform.position + base.muzzleTransform.up, base.muzzleTransform.rotation);
                        spinEffectInstance = _emh_spinEffectInstance.gameObject;
                    }
                    spinEffectInstance.transform.parent = base.muzzleTransform;
                    spinEffectInstance.transform.localScale = Vector3.one;
                }
                if ((bool)spinImpactEffectPrefab)
                {
                    if (!EffectManager.ShouldUsePooledEffect(spinImpactEffectPrefab))
                    {
                        spinImpactEffectInstance = UnityEngine.Object.Instantiate(spinImpactEffectPrefab, base.isInDualWield? base.muzzleTransform.position: base.muzzleTransform.position + base.muzzleTransform.up, base.muzzleTransform.rotation);
                    }
                    else
                    {
                        // + new Vector3(-0.1198245f, 1.320812f, -3.0570632f)
                        _emh_spinImpactEffectInstance = EffectManager.GetAndActivatePooledEffect(spinImpactEffectPrefab, base.isInDualWield ? base.muzzleTransform.position : base.muzzleTransform.position + base.muzzleTransform.up, base.muzzleTransform.rotation);
                        spinImpactEffectInstance = _emh_spinImpactEffectInstance.gameObject;
                    }
                    spinImpactEffectInstance.transform.parent = base.muzzleTransform;
                    spinImpactEffectInstance.transform.localScale = Vector3.one;
                    spinImpactEffectInstance.gameObject.SetActive(value: false);
                }
            }
            attack.isCrit = Util.CheckRoll(critStat, base.characterBody.master);

            //

            // Spear

            spearCooldown = 0f;

            // Scrap

            if (!gameObject.GetComponent<ScrapCooldownTracker>())
            {
                gameObject.AddComponent<ScrapCooldownTracker>();
            }

            scrapCooldownTracker = gameObject.GetComponent<ScrapCooldownTracker>();

            scrapCooldownTracker.GetSkillFamilyStock(SkillCatalog.GetSkillFamilyName(base.activatorSkillSlot.skillFamily.catalogIndex));
        }

        //This method runs once at the end
        //Here, we are doing nothing
        public override void OnExit()
        {
            base.OnExit();
            
            // Saw

            fireAge = 0;

            Util.PlaySound(spinDownSoundString, base.gameObject);

            if (!base.isInDualWield)
            {
                PlayAnimation("Gesture, Additive Gun", EmptyStateHash);
                PlayAnimation("Gesture, Additive", ExitBuzzsawStateHash);
            }
            if ((bool)spinEffectInstance)
            {
                if (!EffectManager.UsePools)
                {
                    EntityState.Destroy(spinEffectInstance);
                }
                else
                {
                    if (_emh_spinEffectInstance != null && _emh_spinEffectInstance.OwningPool != null)
                    {
                        _emh_spinEffectInstance.OwningPool.ReturnObject(_emh_spinEffectInstance);
                    }
                    else
                    {
                        EntityState.Destroy(spinEffectInstance);
                    }
                    _emh_spinEffectInstance = null;
                }
            }
            if ((bool)spinImpactEffectInstance)
            {
                EntityState.Destroy(spinImpactEffectInstance);
            }

            //
        }

        //FixedUpdate() runs almost every frame of the skill
        //Here, we end the skill once it exceeds its intended duration
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            // Saw
            fireAge += GetDeltaTime();
            base.characterBody.SetAimTimer(2f);
            attackSpeedStat = base.characterBody.attackSpeed;
            fireFrequency = baseFireFrequency * attackSpeedStat;
            if (fireAge >= 1f / fireFrequency && base.isAuthority)
            {
                fireAge = 0f;
                attack.ResetIgnoredHealthComponents();
                attack.isCrit = base.characterBody.RollCrit();
                sawBase.transform.localScale = new Vector3(1, 1, 1);
                hitOverlapLastTick = attack.Fire();
                if (hitOverlapLastTick)
                {
                    Vector3 normalized = (attack.lastFireAverageHitPosition - GetAimRay().origin).normalized;
                    if ((bool)base.characterMotor)
                    {
                        base.characterMotor.ApplyForce(normalized * selfForceMagnitude);
                    }

                    //Util.PlaySound(impactSoundString, base.gameObject);

                    if (!base.isInDualWield)
                    {
                        PlayAnimation("Gesture, Additive", ImpactBuzzsawStateHash);
                    }
                }
                //base.characterBody.AddSpreadBloom(spreadBloomValue);
                if (!IsKeyDownAuthority() || (object)base.skillDef != base.activatorSkillSlot.skillDef)
                {
                    outer.SetNextStateToMain();
                }
            }
            spinImpactEffectInstance.gameObject.SetActive(hitOverlapLastTick);

            // Spear


            spearDuration = spearBaseDuration / attackSpeedStat;

            if (spearCooldown <= 0f)
            {

                StartAimMode(GetAimRay(), 3f);
                DoSpearFireEffects();
                //PlaySpearFireAnimation();
                //AddRecoil(-1f * recoilAmplitudeY, -1.5f * recoilAmplitudeY, -1f * recoilAmplitudeX, 1f * recoilAmplitudeX);
                if (base.isAuthority)
                {
                    BulletAttack bulletAttack = GenerateSpearBulletAttack(GetAimRay());
                    ModifySpearBullet(bulletAttack);
                    bulletAttack.Fire();
                    //OnFireBulletAuthority(aimRay);
                }

                //base.characterBody.SetSpreadBloom(1f, canOnlyIncreaseBloom: false);
                //AddRecoil(-0.6f * recoilAmplitude, -0.8f * recoilAmplitude, -0.1f * recoilAmplitude, 0.1f * recoilAmplitude);
                /*if (!((IToolbotPrimarySkillState)this).isInDualWield)
                {
                    PlayAnimation("Gesture, Additive", FireSpearStateHash, FireSpearParamHash, duration);
                }
                else
                {
                    BaseToolbotPrimarySkillStateMethods.PlayGenericFireAnim(this, base.gameObject, base.skillLocator, spearDuration);
                }*/
                spearCooldown = spearDuration;
            } else
            {
                spearCooldown -= 0.0166667f;
            }

            // Scrap

            if(scrapCooldownTracker && scrapCooldownTracker.GetSkillFamilyStock(SkillCatalog.GetSkillFamilyName(base.activatorSkillSlot.skillFamily.catalogIndex)) > 0 && scrapDelayBeforeFiringProjectile <= 0)
            {
                FireScrapBulletAttack();
                ScrapDoFireEffects();

                scrapCooldownTracker.RemoveSkillFamilyStock(SkillCatalog.GetSkillFamilyName(base.activatorSkillSlot.skillFamily.catalogIndex));

                scrapDelayBeforeFiringProjectile = scrapBaseDelayBeforeFiringProjectile / attackSpeedStat;
            } else
            {
                scrapDelayBeforeFiringProjectile -= 0.0166667f;
            }

            //

            //

            /*if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }*/
        }

        //GetMinimumInterruptPriority() returns the InterruptPriority required to interrupt this skill
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        protected virtual void DoSpearFireEffects()
        {

            Util.PlaySound(spearFireSoundString, base.gameObject);

            if ((bool)muzzleTransform)
            {
                EffectManager.SimpleMuzzleFlash(spearMuzzleFlashPrefab, base.gameObject, spearMuzzleName, transmit: false);
            }
        }

        protected void ModifySpearBullet(BulletAttack bulletAttack)
        {
            bulletAttack.stopperMask = LayerIndex.world.mask;
            bulletAttack.falloffModel = BulletAttack.FalloffModel.None;
            //bulletAttack.muzzleName = spearMuzzleName; //((IToolbotPrimarySkillState)this).muzzleName;
            bulletAttack.damageType.damageSource = DamageSource.Primary;
        }

        protected BulletAttack GenerateSpearBulletAttack(Ray aimRay)
        {
            if (base.isInDualWield)
            {
                if (base.currentHand == -1)
                {
                    handBaseMuzzleName = "DualWieldMuzzleL";
                }
                else if (base.currentHand == 1)
                {
                    handBaseMuzzleName = "DualWieldMuzzleR";
                }
            }

            float num = 0f;
            if ((bool)base.characterBody)
            {
                num = base.characterBody.spreadBloomAngle;
            }
            BulletAttack bulletAttack = new BulletAttack
            {
                aimVector = aimRay.direction,
                origin = aimRay.origin,
                owner = base.gameObject,
                weapon = null,
                bulletCount = (uint)spearBulletCount,
                damage = damageStat * spearDamageCoefficient,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Generic,
                falloffModel = BulletAttack.FalloffModel.Buckshot,
                force = force,
                HitEffectNormal = false,
                procChainMask = default(ProcChainMask),
                procCoefficient = spearProcCoefficient,
                maxDistance = spearMaxDistance,
                radius = spearBulletRadius,
                isCrit = RollCrit(),
                muzzleName = handBaseMuzzleName,
                minSpread = spearMinSpread,
                maxSpread = spearMaxSpread + num,
                hitEffectPrefab = spearHitEffectPrefab,
                smartCollision = spearUseSmartCollision,
                sniper = false,
                spreadPitchScale = spearSpreadPitchScale,
                spreadYawScale = spearSpreadYawScale,
                tracerEffectPrefab = spearTracerEffectPrefab
            };
            //ModifySpearBullet(bulletAttack);
            return bulletAttack;
        }

        protected void FireScrapBulletAttack()
        {
            if (base.isInDualWield)
            {
                if (base.currentHand == -1)
                {
                    handBaseMuzzleName = "DualWieldMuzzleL";
                }
                else if (base.currentHand == 1)
                {
                    handBaseMuzzleName = "DualWieldMuzzleR";
                }
            }

            Ray aimRay = GetAimRay();
            aimRay = ScrapModifyProjectileAimRay(aimRay);
            aimRay.direction = Util.ApplySpread(aimRay.direction, scrapMinSpread, scrapMaxSpread, 1f, 1f, 0f, scrapProjectilePitchBonus);
            FireProjectileInfo fireProjectileInfo = default(FireProjectileInfo);
            fireProjectileInfo.projectilePrefab = scrapProjectile;
            fireProjectileInfo.position = aimRay.origin;
            fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
            fireProjectileInfo.owner = base.gameObject;
            fireProjectileInfo.damage = damageStat * scrapDamageCoefficient;
            fireProjectileInfo.force = force;
            fireProjectileInfo.crit = Util.CheckRoll(critStat, base.characterBody.master);
            FireProjectileInfo fireProjectileInfo2 = fireProjectileInfo;
            ScrapModifyProjectileInfo(ref fireProjectileInfo2);
            ProjectileManager.instance.FireProjectile(fireProjectileInfo2);
        }
        protected void ScrapModifyProjectileInfo(ref FireProjectileInfo fireProjectileInfo)
        {
            fireProjectileInfo.damageTypeOverride = DamageTypeCombo.GenericPrimary;
        }


        protected Ray ScrapModifyProjectileAimRay(Ray projectileRay)
        {
            if (((IToolbotPrimarySkillState)this).isInDualWield)
            {
                Transform muzzleTransform = ((IToolbotPrimarySkillState)this).muzzleTransform;
                if ((bool)muzzleTransform)
                {
                    projectileRay.origin = muzzleTransform.position;
                }
            }
            return projectileRay;
        }

        protected virtual void ScrapDoFireEffects()
        {

            Util.PlaySound(scrapFireSoundString, base.gameObject);

            AddRecoil(-2f * recoilAmplitude, -3f * recoilAmplitude, -1f * recoilAmplitude, 1f * recoilAmplitude);
            if ((bool)muzzleflashSmokeRing)
            {
                EffectManager.SimpleMuzzleFlash(muzzleflashSmokeRing, base.gameObject, handBaseMuzzleName, transmit: false);
            }
            //base.characterBody.AddSpreadBloom(bloom);
        }
    }

}
