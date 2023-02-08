using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuTrigger : MonoBehaviour
{
    public GameObject menuPanel;
    public Button cancelButton;
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Open();
        }
    }

    public virtual void Open()
    {
        menuPanel.transform.DOComplete(); 
        menuPanel.SetActive(true);
        menuPanel.transform.DOLocalMove(Vector3.zero, 0.5f).SetUpdate(true).SetEase(Ease.OutBack);
        Player.instance.SetCanMove(false);
        cancelButton.interactable = true;
        cancelButton.Select();
    }
    
    public virtual void Close()
    {
        EventSystem.current.SetSelectedGameObject(null);
        cancelButton.interactable = false;
        Player.instance.SetCanMove(true);
        menuPanel.transform.DOLocalMove(new Vector3(0, -1080, 0), 0.5f).SetUpdate(true).SetEase(Ease.InBack).onComplete += () =>
        {
            menuPanel.SetActive(false);
        };
    }
}
