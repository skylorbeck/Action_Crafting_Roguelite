using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalDoor : MonoBehaviour
{
    [SerializeField] private string targetScene;
    [SerializeField] private GameObject playOptionsPanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button cancelButton;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OpenPortal();
        }
    }

    private void OpenPortal()
    {
        playOptionsPanel.transform.DOComplete(); 
        playOptionsPanel.SetActive(true);
        Player.instance.SetCanMove(false);
        playButton.interactable = true;
        cancelButton.interactable = true;
        playButton.Select();
        playOptionsPanel.transform.DOLocalMove(Vector3.zero, 0.5f).SetUpdate(true).SetEase(Ease.OutBack);
    }

    public void Cancel()
    {
        EventSystem.current.SetSelectedGameObject(null);
        playButton.interactable = false;
        cancelButton.interactable = false;
        Time.timeScale = 1f;
        Player.instance.SetCanMove(true);
        playOptionsPanel.transform.DOLocalMove(new Vector3(0, -1000, 0), 0.5f).SetUpdate(true).SetEase(Ease.InBack).onComplete += () =>
        {
            playOptionsPanel.SetActive(false);
        };
    }
    
    public void LoadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(targetScene);
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
