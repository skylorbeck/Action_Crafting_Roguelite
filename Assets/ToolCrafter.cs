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

    public void Craft()
    {
        ToolStats toolStats = new ToolStats();//TODO: Get from pool
        toolStats.toolType = currentToolType;
        toolStats.toolTier = SaveManager.instance.GetPlayerToolData().GetToolTier();
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
        craftButton.interactable = Player.instance.CanAfford((uint)(10^currentToolTier));
        toolTierText.text = tierNames[currentToolTier];
        toolCostText.text = "x" + (10^currentToolTier);
        
    }

    public void UpdateAll()
    {
        UpdateImage();
        UpdateText();
    }

    public void TierUp()
    {
        currentToolTier++;
        if (currentToolTier > SaveManager.instance.GetPlayerToolData().GetToolTier())
        {
            currentToolTier = SaveManager.instance.GetPlayerToolData().GetToolTier();
        }
        UpdateAll();
    }
    
    public void TierDown()
    {
        currentToolTier--;
        if (currentToolTier < 0)
        {
            currentToolTier = 0;
        }
        UpdateAll();
    }
    
    public void NextClass()
    {
        currentToolType++;
        if (currentToolType > ToolType.Axe)
        {
            currentToolType = ToolType.Pick;
        }
        UpdateAll();
    }
    
    public void PreviousClass()
    {
        currentToolType--;
        if (currentToolType < ToolType.Pick)
        {
            currentToolType = ToolType.Axe;
        }
        UpdateAll();
    }
}
