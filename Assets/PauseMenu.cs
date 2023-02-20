using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    public bool isOpen => menuPanel.activeSelf;
    public bool canOpen = true;
    public Image background;
    public GameObject menuPanel;
    public GameObject quitPanel;
    public Button quitButton;
    public Button resumeButton;

    public void Start()
    {
        instance = this;
        quitPanel.SetActive(false);
        menuPanel.SetActive(false);
    }

    public void Open()
    {
        if (!canOpen || isOpen) return;
        Time.timeScale = 0;
        background.DOFade(0.5f, 0.5f).SetUpdate(true);
        MenuSoundManager.instance.PlayOpen();
        menuPanel.transform.DOComplete();
        menuPanel.SetActive(true);
        menuPanel.transform.DOLocalMove(Vector3.zero, 0.5f).SetUpdate(true).SetEase(Ease.OutBack);
        // Player.instance.SetCanMove(false);
        resumeButton.interactable = true;
        resumeButton.Select();
        quitButton.interactable = true;
    }

    public void Close()
    {
        Time.timeScale = 1;
        background.DOFade(0, 0.5f).SetUpdate(true);
        EventSystem.current.SetSelectedGameObject(null);
        resumeButton.interactable = false;
        quitButton.interactable = false;
        // Player.instance.SetCanMove(true);
        menuPanel.transform.DOLocalMove(new Vector3(0, 1080, 0), 0.5f).SetUpdate(true).SetEase(Ease.InBack)
            .onComplete += () => { menuPanel.SetActive(false); };
    }

    public void TryQuit()
    {
        quitPanel.SetActive(true);
        MenuSoundManager.instance.PlayOpen();
    }

    public void Quit()
    {
        Player.instance.Kill();
        Close();
    }
}