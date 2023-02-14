using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Blacksmith : MenuTrigger
{
    [SerializeField] private UpgradeEntryBrain[] upgradeEntryBrains;
    [SerializeField] private ToolCrafter toolCrafter;

    public override void Open()
    {
        base.Open();
        UpdateEntries();
        toolCrafter.UpdateAll();
        CheckForUpgrades();
    }
    void CheckForUpgrades()
    {
        foreach (UpgradeEntryBrain upgradeEntryBrain in upgradeEntryBrains)
        {
            upgradeEntryBrain.gameObject.SetActive(upgradeEntryBrain.IsUnlocked());
        }
    }
    public void UpdateEntries()
    {
        foreach (var upgradeEntryBrain in upgradeEntryBrains)
        {
            upgradeEntryBrain.UpdateButton();
            upgradeEntryBrain.UpdateText();
        }
    }

    public override void Close()
    {
        base.Close();
        MenuSoundManager.instance.PlayCancel();
    }
}