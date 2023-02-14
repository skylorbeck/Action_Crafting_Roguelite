using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolPreviewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI toolStatsText;
    [SerializeField] private Image toolRenderer;

    [SerializeField] private ToolStats displayedTool;
    public void SetPreview(ToolStats toolStats = null)
    {
        displayedTool = toolStats ?? SaveManager.instance.GetPlayerToolData().GetEquippedTool();
        toolStatsText.text = displayedTool.GetText();
        switch (displayedTool.toolType)
        {
            case ToolType.Pick:
                toolRenderer.sprite = Player.instance.toolRegistry.pickaxeImages[displayedTool.toolTier];
                break;
            case ToolType.Axe:
                toolRenderer.sprite = Player.instance.toolRegistry.axeImages[displayedTool.toolTier];
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SelectThisOne()
    {
        if (displayedTool == null)
        {
            throw new Exception("No tool to select");
        }
        switch (displayedTool.toolType)
        {
            case ToolType.Pick:
                SaveManager.instance.GetPlayerToolData().currentPick = displayedTool;
                break;
            case ToolType.Axe:
                SaveManager.instance.GetPlayerToolData().currentAxe = displayedTool;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
