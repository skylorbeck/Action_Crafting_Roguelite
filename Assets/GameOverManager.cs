using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    [SerializeField] private Image background;
    [SerializeField] private RectTransform canvasPanel;
    
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI woodText;
    [SerializeField] TextMeshProUGUI stoneText;
    [SerializeField] TextMeshProUGUI enemiesKilledText;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] private Button continueButton;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Debug.LogWarning("GameOverManager already exists, destroying object!");
            Destroy(gameObject);
        }
    }
    
    public void Continue()
    {
        SceneManager.LoadScene("TheBlacksmith");
    }

    public async void GameOver()
    {
        RunStats runStats = Player.instance.GetRunStats();

        SaveManager.instance.AddRunToMetaStats(runStats);
        SaveManager.instance.Save();
        background.gameObject.SetActive(true);
        canvasPanel.gameObject.SetActive(true);
        background.DOFade(0.5f, 1f);
        canvasPanel.DOLocalMoveY(0,1f).SetEase(Ease.InBounce);
        continueButton.gameObject.SetActive(true);
        continueButton.interactable = false;
        await Task.Delay(1500);
        DOTween.To(() => timerText.text, x => timerText.text = x, TimerManager.instance.GetElapsedTime(), 0.25f);
        await Task.Delay(100);
        DOTween.To(() => woodText.text, x => woodText.text = x, runStats.woodCollected.ToString(), 0.25f);
        await Task.Delay(100);
        DOTween.To(() => stoneText.text, x => stoneText.text = x, runStats.stoneCollected.ToString(), 0.25f);
        await Task.Delay(100);
        DOTween.To(() => enemiesKilledText.text, x => enemiesKilledText.text = x, runStats.enemiesKilled.ToString(), 0.25f);
        await Task.Delay(100);
        DOTween.To(() => goldText.text, x => goldText.text = x, Player.instance.GetGold().ToString(), 0.25f);
        continueButton.interactable = true;
        continueButton.Select();
        // Debug.Log("Game Over!");
    }
}
