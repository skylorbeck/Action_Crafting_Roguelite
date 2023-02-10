using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitGameDoor : MenuTrigger
{
    public Button quitButton;
    
    public override void Open()
    {
        base.Open();
        quitButton.Select();
        quitButton.interactable = true;
    }

    public override void Close()
    {
        MenuSoundManager.instance.PlayCancel();
        base.Close();
        quitButton.interactable = false;
    }

    public void Quit()
    {
        MenuSoundManager.instance.PlayAccept();
        Application.Quit();
    }
    
}
