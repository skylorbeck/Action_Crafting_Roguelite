using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PerkManager : MonoBehaviour
{
    public static PerkManager instance;
    public List<Perk> allClassPerks = new List<Perk>();
    public List<Perk> preferredPerkPool = new List<Perk>();
    public List<Perk> bonusPerkPool = new List<Perk>();
    [SerializeField] private GameObject perkMenu;
    public bool perkMenuOpen => perkMenu.activeSelf;
    [SerializeField] private Button[] PerkButtons;
    [SerializeField] private PerkDisplay[] perkDisplays;
    [SerializeField] private Button closePerkMenuButton;

    void Start()
    {
        instance = this;

    }

    //TODO value owned perks more than new perks
    public void ShowPerkMenu()
    {
        PauseMenu.instance.canOpen = false;
        MenuSoundManager.instance.PlayOpen();
        perkMenu.SetActive(true);
        perkMenu.transform.DOKill();
        perkMenu.transform.DOLocalMove(Vector3.zero, 0.5f).SetUpdate(true).SetEase(Ease.OutBack);
        Time.timeScale = 0;
        List<Perk> perks = new List<Perk>(Player.instance.classRegistry
            .GetClass((int)SaveManager.instance.GetPlayerToolData().GetEquippedType()).perkList);
        perks.AddRange(allClassPerks);
        perks.AddRange(bonusPerkPool);
        perks.RemoveAll(perk => Player.instance.equippedPerks.Contains(perk));
        foreach (PerkDisplay perkDisplay in perkDisplays)
        {
            perkDisplay.gameObject.SetActive(false);
        }


        if (perks.Count == 0)
        {
            closePerkMenuButton.gameObject.SetActive(true);
            closePerkMenuButton.interactable = true;
        }
        else
        {
            closePerkMenuButton.gameObject.SetActive(false);
        }

        if (perks.Count < PerkButtons.Length)
        {
            for (int i = 0; i < perks.Count; i++)
            {
                perkDisplays[i].gameObject.SetActive(true);
                PerkButtons[i].interactable = true;
                perkDisplays[i].SetPerk(perks[i]);
            }

            return;
        }

        List<int> randomPerkIndexes = new List<int>();

        for (int i = 0; i < PerkButtons.Length; i++)
        {
            PerkButtons[i].interactable = false;
            int randomPerkIndex = Random.Range(0, perks.Count);
            while (randomPerkIndexes.Contains(randomPerkIndex))
            {
                randomPerkIndex = Random.Range(0, perks.Count);
            }

            perkDisplays[i].gameObject.SetActive(true);
            PerkButtons[i].interactable = true;
            perkDisplays[i].SetPerk(perks[randomPerkIndex]);
            randomPerkIndexes.Add(randomPerkIndex);
        }
    }

    public void ClosePerkMenu()
    {
        MenuSoundManager.instance.PlayAccept();
        Time.timeScale = 1f;
        perkMenu.transform.DOLocalMove(new Vector3(0, 1000, 0), 0.5f).SetUpdate(true).SetEase(Ease.InBack)
            .OnComplete(() => perkMenu.SetActive(false));
        foreach (Button button in PerkButtons)
        {
            button.interactable = false;
        }

        closePerkMenuButton.interactable = false;
        Player.instance.CheckForLevelUp();
        PauseMenu.instance.canOpen = true;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {

    }
}