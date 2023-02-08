using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TownCenter : MenuTrigger
{
    [SerializeField] protected uint experience = 0;
    [SerializeField] protected float experienceScale = 1.1f;
    [SerializeField] protected uint experienceToNextLevel = 100;
    [SerializeField] protected uint level = 0;
    [SerializeField] protected Image experienceBar;
    [SerializeField] protected TextMeshProUGUI levelText;

    [SerializeField] protected Button upgradeTownButton;

    public override void Open()
    {
        base.Open();
        upgradeTownButton.interactable = true;
    }

    public override void Close()
    {
        base.Close();
        upgradeTownButton.interactable = false;
    }

    public void Test()
    {
        AddExperience((uint)Mathf.RoundToInt(experienceToNextLevel*Random.value));
    }
    

    public void AddExperience(uint experienceValue)
    {
        experience += experienceValue;
        // PopupManager.instance.SpawnExpNumber((int)experienceValue);
        CheckForLevelUp();
    }

    public void CheckForLevelUp()
    {
        if (experience >= experienceToNextLevel)
        {
            experience -= experienceToNextLevel;
            level++;
            experienceToNextLevel = (uint)Mathf.RoundToInt(experienceToNextLevel * experienceScale);
            levelText.text = "Lv. " + level;
        }

        DOTween.Kill(experienceBar.rectTransform);
        DOTween.To(() => experienceBar.rectTransform.sizeDelta.x,
            x => experienceBar.rectTransform.sizeDelta = new Vector2(x, experienceBar.rectTransform.sizeDelta.y),
            (experience / (float)experienceToNextLevel) * 1820, 0.5f);

    }
}