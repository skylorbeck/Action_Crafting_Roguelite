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
        if (upgradeType == UpgradeType.NodeCapacity)
        { 
            return upgradeCost * SaveManager.instance.GetMetaUpgrades().nodeCapacity;
        }
        return upgradeCost;
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
    }

    public bool HasBoughtUpgrade()
    {
        return SaveManager.instance.GetMetaUpgrades().HasBoughtUpgrade(upgradeType);
    }
}