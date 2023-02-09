using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    [SerializeField] private RoundObject[] rounds;
    [SerializeField] private int currentRound = 0;

    public void NextRound()
    {
        currentRound++;
        StartRound();
    }
    
    public void StartRound()
    {
        if (currentRound >= rounds.Length)
        {
            Player.instance.Kill();//TODO end game without killing?
            return;
        }
        RoundObject round = rounds[currentRound];
        EnemyManager.instance.StartRound(round, currentRound);
    }

    private async void Awake()
    {
        StartRound();
        await Task.Delay(1);
        TimerManager.instance.onOneMinute += NextRound;
    }

    private void OnDisable()
    {
        TimerManager.instance.onOneMinute -= NextRound;
    }
}