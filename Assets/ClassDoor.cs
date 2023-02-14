using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClassDoor : MenuTrigger
{
    [SerializeField] private Button nextClassButton;
    [SerializeField] private Button previousClassButton;
    [SerializeField] private TextMeshProUGUI classNameText;
    [SerializeField] private TextMeshProUGUI classDescriptionText;
    [SerializeField] private Image classRenderer;
    [SerializeField] private ImageAnimator classAnimator;
    public override void Open()
    {
        base.Open();
        nextClassButton.interactable = true;
        previousClassButton.interactable = true;
        SetPreview();

    }
    
    public override void Close()
    {
        base.Close();
        MenuSoundManager.instance.PlayAccept();
        nextClassButton.interactable = false;
        previousClassButton.interactable = false;
        Player.instance.UpdateClass();
    }
  
    private void SetPreview()
    {
        ClassObject classObject = Player.instance.classRegistry.GetClass((int)SaveManager.instance.GetPlayerToolData().GetEquippedType());
        classNameText.text = classObject.className;
        classDescriptionText.text = classObject.classDescription;
        classRenderer.sprite = classObject.classSprites[0];
        classAnimator.SetSprites(classObject.classSprites);
    }

    public void NextClass()
    {
        SaveManager.instance.GetPlayerToolData().NextClass();
        SetPreview();
        MenuSoundManager.instance.PlayAccept();
    }
    
    public void PreviousClass()
    {
        SaveManager.instance.GetPlayerToolData().PreviousClass();
        SetPreview();
        MenuSoundManager.instance.PlayAccept();
    }
}
