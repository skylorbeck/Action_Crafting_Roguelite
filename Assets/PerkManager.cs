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
    [SerializeField] private GameObject perkMenu;
    [SerializeField] private Button[] PerkButtons;
    [SerializeField] private PerkDisplay[] perkDisplays;
    [SerializeField] private Button closePerkMenuButton;
    void Start()
    {
         instance = this;
        
    }

    public void ShowPerkMenu()
    {
        perkMenu.SetActive(true);
        perkMenu.transform.DOComplete();
        perkMenu.transform.DOLocalMove(Vector3.zero, 0.5f).SetUpdate(true).SetEase(Ease.OutBack);
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0f, 0.5f).SetUpdate(true);
        
        List<Perk> perks = new List<Perk>(Player.instance.classRegistry.GetClass(Player.instance.classIndex).perkList);
        perks.AddRange(allClassPerks);
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
                perkDisplays[i].SetPerk(perks[i]);
            }
            return;
        }
        
        List<int> randomPerkIndexes = new List<int>();

        for (int i = 0; i < PerkButtons.Length; i++)
        {
            PerkButtons[i].interactable = false;
            int randomPerkIndex =Random.Range(0, perks.Count);
            while (randomPerkIndexes.Contains(randomPerkIndex))
            {
                randomPerkIndex = Random.Range(0, perks.Count);
            }
            perkDisplays[i].gameObject.SetActive(true);
            perkDisplays[i].SetPerk(perks[randomPerkIndex]);
            randomPerkIndexes.Add(randomPerkIndex);
        }
    }
    
    public void ClosePerkMenu()
    {
        Time.timeScale = 1f;
        perkMenu.transform.DOLocalMove(new Vector3(0, 1000, 0), 0.5f).SetUpdate(true).SetEase(Ease.InBack).OnComplete(() => perkMenu.SetActive(false));
        foreach (Button button in PerkButtons)
        {
            button.interactable = false;
        }
        closePerkMenuButton.interactable = false;
        Player.instance.CheckForLevelUp();
    }
    
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}
