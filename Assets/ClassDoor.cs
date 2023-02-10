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
//TODO weapon swapping
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
        PlayerPrefs.SetInt("classIndex", Player.instance.classIndex);
        PlayerPrefs.SetInt("weaponIndex", Player.instance.weaponIndex);
        PlayerPrefs.Save();
        Player.instance.UpdateClass();
    }
  
    private void SetPreview()
    {
        ClassObject classObject = Player.instance.classRegistry.GetClass(Player.instance.classIndex);
        classNameText.text = classObject.className;
        classDescriptionText.text = classObject.classDescription;
        classRenderer.sprite = classObject.classSprites[0];
        classAnimator.SetSprites(classObject.classSprites);
    }

    public void NextClass()
    {
        Player.instance.classIndex++;
        if (Player.instance.classIndex >= Player.instance.classRegistry.classList.Count)//TODO this won't work with locked classes
        {
            Player.instance.classIndex = 0;
        }
        SetPreview();
        MenuSoundManager.instance.PlayAccept();
    }
    
    public void PreviousClass()
    {
        Player.instance.classIndex--;
        if (Player.instance.classIndex < 0)
        {
            Player.instance.classIndex = Player.instance.classRegistry.classList.Count - 1;
        }
        SetPreview();
        MenuSoundManager.instance.PlayAccept();
    }
}
