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

    public PerkStatModifiers perkStatModifiers = new PerkStatModifiers();
    
    public virtual void OnPerkEquipped()
    {
        Player.instance.AddPerk(this);
    }
}
