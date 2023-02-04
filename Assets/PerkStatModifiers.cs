using System;
using UnityEngine;

namespace PerkSystem
{
    [Serializable]
    public class PerkStatModifiers
    {//TODO make all of these work
        public uint healthFlatBonus;
        public uint damageFlatBonus;
        public float damageMultiplierBonus;
        public float attackSpeedMultiplierBonus;
        public float movementSpeedMultiplierBonus;
        public float critChanceFlatBonus;
        public float critDamageMultiplierBonus;
        public float projectileSpeedMultiplierBonus;
        public float projectileSizeMultiplierBonus;
        public uint projectileCountFlatBonus;
        public float areaOfEffectMultiplierBonus;
        public float experienceValueMultiplierBonus;
        public float goldValueMultiplierBonus;
        public bool nodesExplode;
        public bool enemiesExplode;

        public void Add(PerkStatModifiers statsToAdd)
        {
            healthFlatBonus += statsToAdd.healthFlatBonus;
            damageFlatBonus += statsToAdd.damageFlatBonus;
            damageMultiplierBonus += statsToAdd.damageMultiplierBonus;
            attackSpeedMultiplierBonus += statsToAdd.attackSpeedMultiplierBonus;
            movementSpeedMultiplierBonus += statsToAdd.movementSpeedMultiplierBonus;
            critChanceFlatBonus += statsToAdd.critChanceFlatBonus;
            critDamageMultiplierBonus += statsToAdd.critDamageMultiplierBonus;
            projectileSpeedMultiplierBonus += statsToAdd.projectileSpeedMultiplierBonus;
            projectileSizeMultiplierBonus += statsToAdd.projectileSizeMultiplierBonus;
            projectileCountFlatBonus += statsToAdd.projectileCountFlatBonus;
            areaOfEffectMultiplierBonus += statsToAdd.areaOfEffectMultiplierBonus;
            experienceValueMultiplierBonus += statsToAdd.experienceValueMultiplierBonus;
            goldValueMultiplierBonus += statsToAdd.goldValueMultiplierBonus;
            nodesExplode = statsToAdd.nodesExplode;
            enemiesExplode = statsToAdd.enemiesExplode;
        }

        public void Remove(PerkStatModifiers statsToRemove)
        {
            healthFlatBonus -= statsToRemove.healthFlatBonus;
            damageFlatBonus -= statsToRemove.damageFlatBonus;
            damageMultiplierBonus -= statsToRemove.damageMultiplierBonus;
            attackSpeedMultiplierBonus -= statsToRemove.attackSpeedMultiplierBonus;
            movementSpeedMultiplierBonus -= statsToRemove.movementSpeedMultiplierBonus;
            critChanceFlatBonus -= statsToRemove.critChanceFlatBonus;
            critDamageMultiplierBonus -= statsToRemove.critDamageMultiplierBonus;
            projectileSpeedMultiplierBonus -= statsToRemove.projectileSpeedMultiplierBonus;
            projectileSizeMultiplierBonus -= statsToRemove.projectileSizeMultiplierBonus;
            projectileCountFlatBonus -= statsToRemove.projectileCountFlatBonus;
            areaOfEffectMultiplierBonus -= statsToRemove.areaOfEffectMultiplierBonus;
            experienceValueMultiplierBonus -= statsToRemove.experienceValueMultiplierBonus;
            goldValueMultiplierBonus -= statsToRemove.goldValueMultiplierBonus;
            nodesExplode = statsToRemove.nodesExplode;
            enemiesExplode = statsToRemove.enemiesExplode;
        }
    }
}