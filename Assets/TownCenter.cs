using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TownCenter : MenuTrigger
{
    [SerializeField] protected TownStats townStats;
    [SerializeField] protected Image experienceBar;
    [SerializeField] protected Image experienceBarBG;
    [SerializeField] protected TextMeshProUGUI levelText;
    [SerializeField] protected uint expPerDonate = 10;
    [SerializeField] protected Button upgradeTownButton;

    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite[] sprites;
    
    [SerializeField] public float experienceScale = 2.5f;
    
    [SerializeField] public TMP_InputField inputField;
    public IEnumerator Start()
    {
        yield return new WaitUntil(() => SaveManager.instance != null);
        SetStats();
        UpdateVisuals();
    }

    public void SetStats()
    {
        this.townStats = SaveManager.instance.GetTownStats();
    }

    public override void Open()
    {
        base.Open();
        upgradeTownButton.interactable = Player.instance.GetRunStats().woodCollected >= 100 &&
                                         Player.instance.GetRunStats().stoneCollected >= 100;
    }

    public override void Close()
    {
        base.Close();
        MenuSoundManager.instance.PlayCancel();
        upgradeTownButton.interactable = false;
    }

    [Obsolete]
    public void Test()
    {
        AddExperience((uint)Mathf.RoundToInt(townStats.experienceToNextLevel * Random.value));
    }


    public void SubmitResources()
    {
        RunStats runStats = Player.instance.GetRunStats();
        if (runStats.woodCollected > 100 && runStats.stoneCollected > 100)
        {
            runStats.woodCollected -= 100;
            runStats.stoneCollected -= 100;
            AddExperience(expPerDonate);
            MenuSoundManager.instance.PlayAccept();
        }
        else
        {
            Debug.Log("Not enough resources"); //TODO: Add UI feedback
        }

        upgradeTownButton.interactable = Player.instance.GetRunStats().woodCollected >= 100 &&
                                         Player.instance.GetRunStats().stoneCollected >= 100;
        UpdateVisuals();
    }

    public void AddExperience(uint experienceValue)
    {
        townStats.experience += experienceValue;
        PopupManager.instance.SpawnExpNumber((int)experienceValue);
        CheckForLevelUp();
    }

    public void CheckForLevelUp()
    {
        if (townStats.experience >= townStats.experienceToNextLevel)
        {
            townStats.experience -= townStats.experienceToNextLevel;
            townStats.level++;
            townStats.experienceToNextLevel =
                (uint)Mathf.RoundToInt(townStats.experienceToNextLevel * experienceScale);
            experienceBarBG.DOColor(Color.yellow, 0.25f).OnComplete(() => experienceBarBG.DOColor(Color.white, 0.5f));
            MenuSoundManager.instance.PlayLevelUp();
            CheckForLevelUp();
        }

        UpdateVisuals();

        SaveManager.instance.GetTownStats().SetExperience(townStats.experience);
        SaveManager.instance.GetTownStats().SetLevel(townStats.level);
        SaveManager.instance.GetTownStats().SetExperienceToNextLevel(townStats.experienceToNextLevel);
        SaveManager.instance.SetMetaStats(Player.instance.GetRunStats());
        SaveManager.instance.Save();
    }

    public void UpdateVisuals()
    {
        Player.instance.UpdateResourceText();
        levelText.text = "Lv. " + townStats.level;
        DOTween.Kill(experienceBar.rectTransform);
        DOTween.To(() => experienceBar.rectTransform.sizeDelta.x,
            x => experienceBar.rectTransform.sizeDelta = new Vector2(x, experienceBar.rectTransform.sizeDelta.y),
            (townStats.experience / (float)townStats.experienceToNextLevel) * 1820, 0.5f);
            
        spriteRenderer.sprite = townStats.level >= sprites.Length ? sprites[^1] : sprites[townStats.level];
    }

    public void ValidateResourceBundles(string value)
    {
        
        int.TryParse(value, out int result);
        if (result < 0)
        {
            result = 0;
        }
        // make sure the player has 10x result of wood and stone
        if (result > (int)Player.instance.GetRunStats().woodCollected / 10)
        {
            result = (int)(Player.instance.GetRunStats().woodCollected / 10);
        }
        if (result > (int)Player.instance.GetRunStats().stoneCollected / 10)
        {
            result = (int)(Player.instance.GetRunStats().stoneCollected / 10);
        }
        
        inputField.text = result.ToString();
        upgradeTownButton.interactable = result > 0;
    }
    
    public void DonateResourceBundles()
    {
        int.TryParse(inputField.text, out int result);
        Player.instance.GetRunStats().woodCollected -= (uint)result * 10;
        PopupManager.instance.SpawnWoodNumber(-result * 10);
        Player.instance.GetRunStats().stoneCollected -= (uint) result * 10;
        PopupManager.instance.SpawnStoneNumber(-result * 10);
        AddExperience((uint)(result * expPerDonate));
        MenuSoundManager.instance.PlayAccept();
        UpdateVisuals();
        ValidateResourceBundles("0");
    }
}