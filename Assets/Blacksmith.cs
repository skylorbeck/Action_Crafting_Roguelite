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

    public override void Open()
    {
        base.Open();
        buyNewToolButton.interactable = true;
    }
    
    public override void Close()
    {
        base.Close();
        MenuSoundManager.instance.PlayCancel();
        buyNewToolButton.interactable = false;
    }
}