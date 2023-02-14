using System;
using UnityEngine;

[Serializable]
public class ToolStats{
    public uint version = 1;
    public ToolType toolType = ToolType.Pick;
    public int toolTier = 0;
    
    // public Perk startsWithPerk = null;//TODO Perk Lookup Table for serialization
    public string name = "Tool";
    
    public float damageScale = 1f;
    public float attackSpeedBonus = 0f;
    public float movementSpeedBonus = 0f;
    public float critChanceBonus = 0f;
    public float critDamageBonus = 0f;
    public float projectileSpeedBonus = 0f;
    public float projectileSizeBonus = 0f;
    public int projectileCountBonus = 0;
    public float projectileLifeTimeBonus = 0f;
    public float areaOfEffectBonus = 0f;

    // unique id
    public string Guid = System.Guid.NewGuid().ToString();
    
    public ToolStats()
    {
    }
    
    public ToolStats(ToolStats toolStats = null)
    {
        if (toolStats==null|| toolStats.version < 1) return;
        name = toolStats.name;
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

    public ToolStats(ToolType currentToolType, int currentToolTier)
    {
        toolType = currentToolType;
        toolTier = currentToolTier;
        Guid = System.Guid.NewGuid().ToString();
        name = Guid.Substring(0, 6);
        
    }

    public void Randomize()
    {
        damageScale = UnityEngine.Random.Range(0.9f+(0.05f*toolTier), 1.2f+(0.15f*toolTier));
        attackSpeedBonus = UnityEngine.Random.Range(-0.1f+(0.05f*toolTier), 0.1f+(0.1f*toolTier));
        movementSpeedBonus = UnityEngine.Random.Range(-0.1f+(0.05f*toolTier), 0.1f+(0.1f*toolTier));
        critChanceBonus = UnityEngine.Random.Range(-0.1f+(0.05f*toolTier), 0.1f+(0.1f*toolTier));
        critDamageBonus = UnityEngine.Random.Range(-0.1f+(0.05f*toolTier), 0.1f+(0.1f*toolTier));
        projectileSpeedBonus = UnityEngine.Random.Range(-0.1f+(0.05f*toolTier), 0.1f+(0.1f*toolTier));
        projectileSizeBonus = UnityEngine.Random.Range(-0.1f+(0.05f*toolTier), 0.1f+(0.1f*toolTier));
        projectileCountBonus = UnityEngine.Random.Range(0, 2+toolTier);
        projectileLifeTimeBonus = UnityEngine.Random.Range(-0.1f+(0.05f*toolTier), 0.1f+(0.1f*toolTier));
        areaOfEffectBonus = UnityEngine.Random.Range(-0.1f+(0.05f*toolTier), 0.1f+(0.1f*toolTier));
    }

    public string GetText()
    {
        string text = "";
        text += "Damage: " + damageScale.ToString("P") + "\n";
        //add + to positive values and - to negative values
        text += "Attack Speed: " + (attackSpeedBonus >= 0 ? "+" : "") + attackSpeedBonus.ToString("P") + "\n";
        text += "Movement Speed: " + (movementSpeedBonus >= 0 ? "+" : "") + movementSpeedBonus.ToString("P") + "\n";
        text += "Crit Chance: " + (critChanceBonus >= 0 ? "+" : "") + critChanceBonus.ToString("P") + "\n";
        text += "Crit Damage: " + (critDamageBonus >= 0 ? "+" : "") + critDamageBonus.ToString("P") + "\n";
        text += "Projectile Speed: " + (projectileSpeedBonus >= 0 ? "+" : "") + projectileSpeedBonus.ToString("P") + "\n";
        text += "Projectile Size: " + (projectileSizeBonus >= 0 ? "+" : "") + projectileSizeBonus.ToString("P") + "\n";
        text += "Projectile Count: " + (projectileCountBonus >= 0 ? "+" : "") + projectileCountBonus + "\n";
        text += "Projectile Time: " + (projectileLifeTimeBonus >= 0 ? "+" : "") + projectileLifeTimeBonus.ToString("0.00") +" s" + "\n";
        text += "Area: " + (areaOfEffectBonus >= 0 ? "+" : "") + areaOfEffectBonus.ToString("P") + "\n";
        return text;
    }
}
[Serializable]
public enum ToolType
{
    Pick,
    Axe
}
