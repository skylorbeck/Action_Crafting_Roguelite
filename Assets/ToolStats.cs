using System;

[Serializable]
public class ToolStats{
    public uint version = 1;
    public ToolType toolType = ToolType.Pick;
    public int toolTier = 0;
    
    // public Perk startsWithPerk = null;//TODO Perk Lookup Table for serialization
    public float damageScale = 1f;
    public float attackSpeedBonus = 0f;
    public float movementSpeedBonus = 0f;
    public float critChanceBonus = 0f;
    public float critDamageBonus = 0f;
    public float projectileSpeedBonus = 0f;
    public float projectileSizeBonus = 0f;
    public int projectileCountBonus = 0;
    public float areaOfEffectBonus = 0f;
    public float projectileLifeTimeBonus = 0f;
    // unique id
    public string Guid = System.Guid.NewGuid().ToString();
    
    public ToolStats(ToolStats toolStats = null)
    {
        if (toolStats==null|| toolStats.version < 1) return;
        toolType = toolStats.toolType;
        toolTier = toolStats.toolTier;
        damageScale = toolStats.damageScale;
        attackSpeedBonus = toolStats.attackSpeedBonus;
        movementSpeedBonus = toolStats.movementSpeedBonus;
        critChanceBonus = toolStats.critChanceBonus;
        critDamageBonus = toolStats.critDamageBonus;
        projectileSpeedBonus = toolStats.projectileSpeedBonus;
        projectileSizeBonus = toolStats.projectileSizeBonus;
        projectileCountBonus = toolStats.projectileCountBonus;
        areaOfEffectBonus = toolStats.areaOfEffectBonus;
        projectileLifeTimeBonus = toolStats.projectileLifeTimeBonus;
        Guid = toolStats.Guid;
    }
    
}
[Serializable]
public enum ToolType
{
    Pick,
    Axe
}
