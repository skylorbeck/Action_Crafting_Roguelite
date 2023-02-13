using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Blacksmith : MenuTrigger
{
    [SerializeField] private Button buyNewToolButton;
    [SerializeField] private UpgradeEntryBrain[] upgradeEntryBrains;

    public override void Open()
    {
        base.Open();
        buyNewToolButton.interactable = true;
        UpdateEntries();
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
        buyNewToolButton.interactable = false;
    }
}