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
    [SerializeField] private Slider effectVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    public override void Open()
    {
        base.Open();
        sillyToggle.isOn = PlayerPrefs.GetInt("sillyMode", 0) == 1;
        effectVolumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("effectVolume", 1));
        musicVolumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("musicVolume", 1));
        deleteSaveButton.interactable = false;
        areYouSurePanel.SetActive(false);
        areYouSureButton.interactable = true;
    }
    
    public override void Close()
    {
        base.Close();
        MenuSoundManager.instance.PlayAccept();
        PlayerPrefs.Save();
        deleteSaveButton.interactable = false;
        areYouSureButton.interactable = false;
        areYouSurePanel.SetActive(false);
    }

    public void AreYouSure()
    {
        MenuSoundManager.instance.PlayCancel();
        deleteSaveButton.interactable = true;
        areYouSurePanel.SetActive(true);
    }

    public void ImNotSure()
    {
        MenuSoundManager.instance.PlayCancel();
        deleteSaveButton.interactable = false;
        areYouSurePanel.SetActive(false);
    }
    
    public void DeleteSave()
    {

        SaveManager.instance.Reset();
        SaveManager.instance.Save();
        Close();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetSillyMode(bool sillyMode)
    {
        MenuSoundManager.instance.PlayAccept();
        PlayerPrefs.SetInt("sillyMode", sillyMode ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    public void SetEffectVolume(float volume)
    {
        PlayerPrefs.SetFloat("effectVolume", volume);
    }
    
    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    
}
