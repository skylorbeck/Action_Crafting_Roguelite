using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    public UnityAction onOneSecond;
    public UnityAction onOneMinute;
    public UnityAction onFiveMinutes;
    public UnityAction onTenMinutes;
    public UnityAction onThirtyMinutes;
    public UnityAction onOneHour;

    [SerializeField] private float oneSecond = 0f;
    [SerializeField] private uint seconds = 0;
    [SerializeField] private uint minutes = 0;
    [SerializeField] private uint fiveMinutes = 0;
    [SerializeField] private uint tenMinutes = 0;
    [SerializeField] private uint thirtyMinutes = 0;
    [SerializeField] private uint hours = 0;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogWarning("TimerManager already exists, destroying object!");
            Destroy(gameObject);
        }
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        oneSecond += Time.fixedDeltaTime;
        
        if (!(oneSecond >= 1f)) return;
        onOneSecond?.Invoke();
        oneSecond = 0f;
        seconds++;
        
        if (seconds < 60) return;
        onOneMinute?.Invoke();
        seconds = 0;
        minutes++;
        
        if (minutes < 5) return;
        onFiveMinutes?.Invoke();
        minutes = 0;
        fiveMinutes++;
        
        if (fiveMinutes < 2) return;
        onTenMinutes?.Invoke();
        fiveMinutes = 0;
        tenMinutes++;
        
        if (tenMinutes < 3) return;
        onThirtyMinutes?.Invoke();
        tenMinutes = 0;
        thirtyMinutes++;
        
        if (thirtyMinutes < 2) return;
        onOneHour?.Invoke();
        thirtyMinutes = 0;
        hours++;
    }

    public string GetElapsedTime()
    {
        return $"{hours:00}:{(thirtyMinutes*30)+(tenMinutes*10)+(fiveMinutes*5)+(minutes):00}:{seconds:00}";
    }
}