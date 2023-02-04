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
    public Image perkBorder;
    public Sprite[] perkBorderSprites;
    
    public void SetPerk(Perk perk)
    {
        this.perk = perk;
        perkNameText.text = perk.perkName;
        perkDescriptionText.text = perk.perkDescription;
        perkIcon.sprite = perk.perkIcon;
        perkBorder.sprite = perkBorderSprites[perk.perkLevel];
    }
    
    public void EquipPerk()
    {
        perk.OnPerkEquipped();
        PerkManager.instance.ClosePerkMenu();
    }
}
