using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnScreenTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    void Start()
    {
        TimerManager.instance.onOneSecond += UpdateText;
    }

    private void UpdateText()
    {
        text.text = TimerManager.instance.GetElapsedTime();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }
}
