using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace PerkSystem
{
    [Serializable]
    public class PerkStatModifiers
    {
        public int healthFlatBonus;
        public int damageFlatBonus;
        public float damageMultiplierBonus;
        public float attackSpeedMultiplierBonus;
        public float movementSpeedMultiplierBonus;
        public float critChanceFlatBonus;//TODO
        public float critDamageMultiplierBonus;//TODO
        public float projectileSpeedMultiplierBonus;
        public float projectileSizeMultiplierBonus;
        public int projectileCountFlatBonus;
        public float areaOfEffectMultiplierBonus;
        public float experienceValueMultiplierBonus;
        public float goldValueMultiplierBonus;
        public int resourceDropFlatBonus;
        public float enemyHealthMultiplier;
        public float enemySpeedMultiplier;
        public bool enemiesSpawnPick;
        public bool stoneNodesExplode;
        public bool splitPicks;//TODO
        public bool splitAxes;//TODO
        public bool combineAxes;//TODO
        public bool treesBurn;
        

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
            resourceDropFlatBonus += statsToAdd.resourceDropFlatBonus;
            enemyHealthMultiplier += statsToAdd.enemyHealthMultiplier;
            enemySpeedMultiplier += statsToAdd.enemySpeedMultiplier;
            enemiesSpawnPick = enemiesSpawnPick || statsToAdd.enemiesSpawnPick;
            stoneNodesExplode = stoneNodesExplode || statsToAdd.stoneNodesExplode;
            splitPicks = splitPicks || statsToAdd.splitPicks;
            splitAxes = splitAxes || statsToAdd.splitAxes;
            combineAxes = combineAxes || statsToAdd.combineAxes;
            treesBurn = treesBurn || statsToAdd.treesBurn;
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
            resourceDropFlatBonus -= statsToRemove.resourceDropFlatBonus;
            enemyHealthMultiplier -= statsToRemove.enemyHealthMultiplier;
            enemySpeedMultiplier -= statsToRemove.enemySpeedMultiplier;
            enemiesSpawnPick = enemiesSpawnPick && !statsToRemove.enemiesSpawnPick;
            stoneNodesExplode = stoneNodesExplode && !statsToRemove.stoneNodesExplode;
            splitPicks = splitPicks && !statsToRemove.splitPicks;
            splitAxes = splitAxes && !statsToRemove.splitAxes;
            combineAxes = combineAxes && !statsToRemove.combineAxes;
            treesBurn = treesBurn && !statsToRemove.treesBurn;
        }
    }
}