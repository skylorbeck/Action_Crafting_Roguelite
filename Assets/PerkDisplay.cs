using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkDisplay : MonoBehaviour
{
    public Perk perk;
    public TextMeshProUGUI perkNameText;
    public TextMeshProUGUI perkDescriptionText;
    public Image perkIcon;
    
    public void SetPerk(Perk perk)
    {
        this.perk = perk;
        perkNameText.text = perk.perkName;
        perkDescriptionText.text = perk.perkDescription;
        perkIcon.sprite = perk.perkIcon;
    }
    
    public void EquipPerk()
    {
        perk.OnPerkEquipped();
        PerkManager.instance.ClosePerkMenu();
    }
}
