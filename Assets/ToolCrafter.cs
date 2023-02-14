using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolCrafter : MonoBehaviour
{ 
    [SerializeField] private ToolType currentToolType = ToolType.Pick;
    [SerializeField] private int currentToolTier = 0;
    [SerializeField] private Image toolImage;
    [SerializeField] private TextMeshProUGUI toolTierText;
    [SerializeField] private String[] tierNames;
    [SerializeField] private TextMeshProUGUI toolCostText;
    [SerializeField] private Button craftButton;

    [SerializeField] private int cost;
    public void Craft()
    {
        //TODO item generator
        MenuSoundManager.instance.PlayAccept();
    }

    public void UpdateImage()
    {
        switch (currentToolType)
        {
            case ToolType.Pick:
                toolImage.sprite = Player.instance.toolRegistry.pickaxeImages[currentToolTier];
                break;
            case ToolType.Axe:
                toolImage.sprite = Player.instance.toolRegistry.axeImages[currentToolTier];
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void UpdateText()
    {
        craftButton.interactable = Player.instance.CanAfford((uint)cost);
        toolTierText.text = tierNames[currentToolTier];
        toolCostText.text = "x" + cost;
    }

    public void UpdateCost()
    {
        cost =  (int)Mathf.Pow(10, currentToolTier+2);
        craftButton.interactable = Player.instance.CanAfford((uint)cost);
    }

    public void UpdateAll()
    {
        UpdateCost();
        UpdateImage();
        UpdateText();
    }

    public void TierUp()
    {
        currentToolTier++;
        if (currentToolTier > SaveManager.instance.GetPlayerToolData().GetToolTier())
        {
            currentToolTier = 0;
        }
        MenuSoundManager.instance.PlayCancel();
        UpdateAll();
    }
    
    public void TierDown()
    {
        currentToolTier--;
        if (currentToolTier < 0)
        {
            currentToolTier = SaveManager.instance.GetPlayerToolData().GetToolTier();
        }
        MenuSoundManager.instance.PlayCancel();
        UpdateAll();
    }
    
    public void NextClass()
    {
        currentToolType++;
        if (currentToolType > ToolType.Axe)
        {
            currentToolType = ToolType.Pick;
        }
        MenuSoundManager.instance.PlayCancel();
        UpdateAll();
    }
    
    public void PreviousClass()
    {
        currentToolType--;
        if (currentToolType < ToolType.Pick)
        {
            currentToolType = ToolType.Axe;
        }
        MenuSoundManager.instance.PlayCancel();
        UpdateAll();
    }
}
