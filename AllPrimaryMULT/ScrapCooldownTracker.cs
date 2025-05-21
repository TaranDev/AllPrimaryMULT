using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.AddressableAssets;
using UnityEngine;
using Rewired.Utils;
using UnityEngine.PlayerLoop;
using BepInEx;

namespace AllPrimaryMULT
{
    internal class ScrapCooldownTracker : MonoBehaviour
    {

        Dictionary<string, ScrapCooldownStats> skillFamilyScraps;

        public void Awake()
        {
            skillFamilyScraps = new Dictionary<string, ScrapCooldownStats>();
        }

        public int GetSkillFamilyStock(string skillFamily)
        {
            if (!skillFamilyScraps.ContainsKey(skillFamily))
            {
                skillFamilyScraps.Add(skillFamily, new ScrapCooldownStats());
            }

            return skillFamilyScraps[skillFamily].currentStock;
        }

        public void RemoveSkillFamilyStock(string skillFamily)
        {
            if (skillFamilyScraps.ContainsKey(skillFamily))
            {
                if(skillFamilyScraps[skillFamily].currentStock > 0)
                {
                    skillFamilyScraps[skillFamily].currentStock -= 1;
                    skillFamilyScraps[skillFamily].cooldownRemaining = 0;
                }
            }
        }

        /*public void ResetSkillFamilyStockCooldown(string skillFamily)
        {
            if (skillFamilyScraps.ContainsKey(skillFamily))
            {
                skillFamilyScraps[skillFamily].cooldownRemaining = 0;
            }
        }*/

        public void FixedUpdate()
        {
            // need to factor in attack speed or smth
            float skillCooldown = 1.5f;
            foreach (ScrapCooldownStats skillFamily in skillFamilyScraps.Values)
            {
                skillFamily.cooldownRemaining += 0.0166667f;
                if (skillFamily.cooldownRemaining > skillCooldown)
                {
                    skillFamily.cooldownRemaining = 0f;
                    skillFamily.currentStock = 4;
                }
            }

        }
        
    }

    internal class ScrapCooldownStats
    {
        public int maxStock = 4;
        public int currentStock;
        public float cooldownRemaining;

        public ScrapCooldownStats()
        {
            cooldownRemaining = 0;
            currentStock = maxStock;
        }
    }
}
