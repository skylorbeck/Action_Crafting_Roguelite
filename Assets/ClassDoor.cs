using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClassDoor : MonoBehaviour
{
    [SerializeField] private GameObject classPanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button nextClassButton;
    [SerializeField] private Button previousClassButton;
    [SerializeField] private TextMeshProUGUI classNameText;
    [SerializeField] private TextMeshProUGUI classDescriptionText;
    [SerializeField] private Image classRenderer;
    [SerializeField] private ImageAnimator classAnimator;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OpenClassPanel();
        }
    }

    public void OpenClassPanel()//TODO weapon swapping
    {
        classPanel.transform.DOComplete();  
        classPanel.SetActive(true);
        Player.instance.SetCanMove(false);
        closeButton.interactable = true;
        nextClassButton.interactable = true;
        previousClassButton.interactable = true;
        closeButton.Select();
        classPanel.transform.DOLocalMove(Vector3.zero, 0.5f).SetUpdate(true).SetEase(Ease.OutBack);

        SetPreview();
    }

    private void SetPreview()
    {
        ClassObject classObject = Player.instance.classRegistry.GetClass(Player.instance.classIndex);
        classNameText.text = classObject.className;
        classDescriptionText.text = classObject.classDescription;
        classRenderer.sprite = classObject.classSprites[0];
        classAnimator.SetSprites(classObject.classSprites);
    }

    public void CloseClassPanel()
    {
        PlayerPrefs.SetInt("classIndex", Player.instance.classIndex);
        PlayerPrefs.Save();
        EventSystem.current.SetSelectedGameObject(null);
        closeButton.interactable = false;
        nextClassButton.interactable = false;
        previousClassButton.interactable = false;
        Time.timeScale = 1f;
        Player.instance.SetCanMove(true);
        Player.instance.UpdateClass();
        classPanel.transform.DOLocalMove(new Vector3(0, -1000, 0), 0.5f).SetUpdate(true).SetEase(Ease.InBack)
            .onComplete += () => { classPanel.SetActive(false); };
        
    }

    public void NextClass()
    {
        Player.instance.classIndex++;
        if (Player.instance.classIndex >= Player.instance.classRegistry.classList.Count)//TODO this won't work with locked classes
        {
            Player.instance.classIndex = 0;
        }
        SetPreview();
    }
    
    public void PreviousClass()
    {
        Player.instance.classIndex--;
        if (Player.instance.classIndex < 0)
        {
            Player.instance.classIndex = Player.instance.classRegistry.classList.Count - 1;
        }
        SetPreview();
    }
}
