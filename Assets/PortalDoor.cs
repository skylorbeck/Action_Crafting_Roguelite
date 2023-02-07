using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalDoor : MenuTrigger
{
    [SerializeField] private string targetScene;
    [SerializeField] private Button playButton;


    public override void Open()
    {
        base.Open();
        playButton.interactable = true;
    }

    public override void Close()
    {
        base.Close();
        playButton.interactable = false;
    }
    
    public void LoadScene()
    {
        SaveManager.instance.SetMetaStats(Player.instance.GetRunStats());
        SaveManager.instance.Save();
        Time.timeScale = 1f;
        SceneManager.LoadScene(targetScene);
    }
  
}
