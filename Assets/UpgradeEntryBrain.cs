using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeEntryBrain : MonoBehaviour
{
    [SerializeField] private UpgradeObject upgradeObject;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI upgradeDescription;
    [SerializeField] private TextMeshProUGUI upgradeCost;
    [SerializeField] private TextMeshProUGUI buyBoughtText;
    [SerializeField] private Color ColorBought;
    
    public void TryBuyUpgrade()
    {
        if (upgradeObject.CanBuyUpgrade())
        {
            upgradeObject.BuyUpgrade();
            MenuSoundManager.instance.PlayAccept();
        } else {
            MenuSoundManager.instance.PlayCancel();
        }
        //This is handled by the blacksmith
        // UpdateButton();
    }

    public void UpdateButton()
    {
        buyButton.interactable = upgradeObject.CanBuyUpgrade();
    }

    public void UpdateText()
    {
        upgradeDescription.text = upgradeObject.upgradeDescription;
        upgradeCost.text = "x" + upgradeObject.upgradeCost;
        if (upgradeObject.HasBoughtUpgrade())
        {
            buyBoughtText.color =ColorBought;
            upgradeCost.color = ColorBought;
            upgradeDescription.color = ColorBought;
            buyBoughtText.text = "Bought";
        }
    }
}
