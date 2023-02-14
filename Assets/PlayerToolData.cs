using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerToolData
{
    public uint version = 1;
    public ToolType equippedType = ToolType.Pick;
    public ToolStats currentPick = new ToolStats(){toolType = ToolType.Pick};
    public ToolStats currentAxe = new ToolStats() { toolType = ToolType.Axe };
    [SerializeField] public uint toolTier = 0;

    public PlayerToolData()
    {
        Reset();
    }

    private void Reset()
    {
        toolTier = 0;
    }

    public uint GetToolTier()
    {
        return toolTier;
    }
    
    
    public void NextClass()
    {
        equippedType++;
        if (equippedType > ToolType.Axe)
        {
            equippedType = ToolType.Pick;
        }
    }
    
    public void PreviousClass()
    {
        equippedType--;
        if (equippedType < ToolType.Pick)
        {
            equippedType = ToolType.Axe;
        }
    }

    public ToolType GetEquippedType()
    {
        return equippedType;
    }

    public ToolStats GetEquippedTool()
    {
        switch (equippedType)
        {
            case ToolType.Pick:
                return currentPick;
            case ToolType.Axe:
                return currentAxe;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}