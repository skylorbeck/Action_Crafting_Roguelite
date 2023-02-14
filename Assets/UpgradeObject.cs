using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeObject", menuName = "UpgradeObject")]
public class UpgradeObject : ScriptableObject
{
    public string upgradeDescription;
    public uint upgradeCost;
    public UpgradeType upgradeType;
    
    public bool CanBuyUpgrade()
    {
        return !SaveManager.instance.GetMetaUpgrades().HasBoughtUpgrade(upgradeType) &&
               Player.instance.CanAfford(GetUpgradeCost());
    }
    
    public uint GetUpgradeCost()
    {
        switch (upgradeType)
        {
            case UpgradeType.NodeCapacity:
                return upgradeCost * SaveManager.instance.GetMetaUpgrades().nodeCapacity;
            case UpgradeType.ToolTier:
                return upgradeCost * (SaveManager.instance.GetPlayerToolData().GetToolTier()+1);
            case UpgradeType.BiggerNodes:
            case UpgradeType.BanishButton:
            case UpgradeType.Minimap:
            default:
                return upgradeCost;
        }
    }

    public void BuyUpgrade()
    {
        Player.instance.SpendGold(GetUpgradeCost());
        SaveManager.instance.GetMetaUpgrades().BuyUpgrade(upgradeType);
    }
    
    public enum UpgradeType
    {
        BiggerNodes,
        BanishButton,
        Minimap,
        NodeCapacity,
        ToolTier,
    }

    public bool HasBoughtUpgrade()
    {
        return SaveManager.instance.GetMetaUpgrades().HasBoughtUpgrade(upgradeType);
    }
}