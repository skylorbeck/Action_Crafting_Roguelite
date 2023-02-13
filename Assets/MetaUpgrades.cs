using System;

[Serializable]
public class MetaUpgrades
{
    public uint version = 1;

    public bool biggerNodes = false;
    public bool banishButton = false;
    public bool minimap = false;
    public uint nodeCapacity = 1;

    //version 1 end


    public void Reset()
    {
        biggerNodes = false;
        banishButton = false;
        minimap = false;
        nodeCapacity = 1;
    }

    public bool InsertSaveData(MetaUpgrades saveFile)
    {
        if (saveFile.version < 1)
        {
            return false;
        }

        biggerNodes = saveFile.biggerNodes;
        banishButton = saveFile.banishButton;
        minimap = saveFile.minimap;
        nodeCapacity = saveFile.nodeCapacity;
        return true;
    }

    public bool HasBoughtUpgrade(UpgradeObject.UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeObject.UpgradeType.BiggerNodes:
                return biggerNodes;
            case UpgradeObject.UpgradeType.BanishButton:
                return banishButton;
            case UpgradeObject.UpgradeType.Minimap:
                return minimap;
            case UpgradeObject.UpgradeType.NodeCapacity:
                return nodeCapacity > 10;
            default:
                return false;
        }
    }

    public void BuyUpgrade(UpgradeObject.UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeObject.UpgradeType.BiggerNodes:
                biggerNodes = true;
                break;
            case UpgradeObject.UpgradeType.BanishButton:
                banishButton = true;
                break;
            case UpgradeObject.UpgradeType.Minimap:
                minimap = true;
                break;
            case UpgradeObject.UpgradeType.NodeCapacity:
                nodeCapacity++;
                break;
            default:
                break;
        }
    }
    
}