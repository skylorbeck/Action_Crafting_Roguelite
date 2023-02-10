using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using PerkSystem;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Perk", menuName = "Perk")]
public class Perk :ScriptableObject
{
    public string perkName;
    public string perkDescription;
    public Sprite perkIcon;
    public int perkCost;
    
    public int perkLevel;
    
    public Perk nextPerk;

    public Tool newTool;//TODO 

    public PerkStatModifiers perkStatModifiers = new PerkStatModifiers();
    
    public virtual void OnPerkEquipped()
    {
        if (newTool!=null)
        {
            Player.instance.EquipTool(newTool);
        }
        if (nextPerk!=null)
        {
            PerkManager.instance.bonusPerkPool.Add(nextPerk);
        }
        Player.instance.AddPerk(this);
        
        if (perkStatModifiers.healthFlatBonus!=0)
        {
            Player.instance.AddHealth(perkStatModifiers.healthFlatBonus);
        }

        if (perkStatModifiers.areaOfEffectMultiplierBonus!=0)
        {
         Player.instance.UpdatePickupSize();   
        }
        
        
    }
    
    
}

