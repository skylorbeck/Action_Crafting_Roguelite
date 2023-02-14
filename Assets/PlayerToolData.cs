using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerToolData
{
    public uint version = 1;
    [SerializeField] public ToolType equippedType = ToolType.Pick;
    [SerializeField] public string equippedGuid = Guid.Empty.ToString();
    [SerializeField] public List<ToolStats> tools = new List<ToolStats>();
    [SerializeField] public int toolTier = 0;

    public PlayerToolData()
    {
        Reset();
    }

    private void Reset()
    {
        equippedType = ToolType.Pick;
        equippedGuid = Guid.Empty.ToString();
        tools = new List<ToolStats>();
        toolTier = 0;
    }

    public int GetToolTier()
    {
        return toolTier;
    }
    
    public ToolType GetEquippedType()
    {
        return equippedType;
    }
    
    public List<ToolStats> GetOfType(ToolType toolType)
    {
        return tools.Where(tool => tool.toolType == toolType).ToList();
    }
    
    public ToolStats GetEquipped()
    {
        return tools.FirstOrDefault(tool => tool.Guid.Equals(equippedGuid));
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
}