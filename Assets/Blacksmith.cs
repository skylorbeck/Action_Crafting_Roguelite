using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Blacksmith : MonoBehaviour
{
    [SerializeField] private GameObject blacksmithPanel;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button buyNewToolButton;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            OpenBlacksmith();
        }
    }

    public void OpenBlacksmith()
    {
        blacksmithPanel.SetActive(true);
        blacksmithPanel.transform.DOLocalMove(Vector3.zero, 0.5f).SetUpdate(true).SetEase(Ease.OutBack);
        Player.instance.SetCanMove(false);
        cancelButton.interactable = true;
        buyNewToolButton.interactable = true;
        cancelButton.Select();
    }
    
    public void CloseBlacksmith()
    {
        EventSystem.current.SetSelectedGameObject(null);
        cancelButton.interactable = false;
        buyNewToolButton.interactable = false;
        Player.instance.SetCanMove(true);
        blacksmithPanel.transform.DOLocalMove(new Vector3(0, -1000, 0), 0.5f).SetUpdate(true).SetEase(Ease.InBack).onComplete += () =>
        {
            blacksmithPanel.SetActive(false);
        };
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void FixedUpdate()
    {

    }
}