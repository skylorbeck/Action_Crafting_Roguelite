using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsDoor : MenuTrigger
{
    [SerializeField] private Toggle sillyToggle;
    [SerializeField] private Button areYouSureButton;
    [SerializeField] private Button deleteSaveButton;
    [SerializeField] private GameObject areYouSurePanel;

    public override void Open()
    {
        base.Open();
        sillyToggle.isOn = PlayerPrefs.GetInt("sillyMode", 0) == 1;
        deleteSaveButton.interactable = false;
        areYouSurePanel.SetActive(false);
        areYouSureButton.interactable = true;
    }
    
    public override void Close()
    {
        base.Close();
        deleteSaveButton.interactable = false;
        areYouSureButton.interactable = false;
        areYouSurePanel.SetActive(false);
    }

    public void AreYouSure()
    {
        deleteSaveButton.interactable = true;
        areYouSurePanel.SetActive(true);
    }
    
    public void DeleteSave()
    {
        SaveManager.instance.ResetTownStats();
        SaveManager.instance.ResetMetaStats();
        SaveManager.instance.Save();
        Close();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetSillyMode(bool sillyMode)
    {
        PlayerPrefs.SetInt("sillyMode", sillyMode ? 1 : 0);
        PlayerPrefs.Save();
    }
    
}
