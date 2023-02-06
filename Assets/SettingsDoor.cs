using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsDoor : MenuTrigger
{
    [SerializeField] private Toggle sillyToggle;
    
    public override void Open()
    {
        base.Open();
        sillyToggle.isOn = PlayerPrefs.GetInt("sillyMode", 0) == 1;
    }
    
    public void SetSillyMode(bool sillyMode)
    {
        PlayerPrefs.SetInt("sillyMode", sillyMode ? 1 : 0);
    }
    
}
