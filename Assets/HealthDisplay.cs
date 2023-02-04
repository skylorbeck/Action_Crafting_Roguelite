using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public static HealthDisplay instance;
    
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    
    public List<Image> hearts = new List<Image>();

    public bool active = true;
    public float heartSpacing = 90;
    public bool compressed = false;
    public float compression = 0.4f;
    public bool shake = false;

    public float shakeStrength = 2f;
    private void Awake()
    {
        instance = this;
        heartSpacing = PlayerPrefs.GetFloat("heartSpacing", 90);
        compressed = PlayerPrefs.GetInt("compressed", 0) == 1;
        compression = PlayerPrefs.GetFloat("compression", 0.4f);
        shake = PlayerPrefs.GetInt("shake", 1) == 1;
        shakeStrength = PlayerPrefs.GetFloat("shakeStrength", 2f);
    }

    public void SetMaxHealth(int health)
    {
        if (!active) return;
        int maxHealth = (int) Math.Round(health / 2f, MidpointRounding.AwayFromZero);
        
        if (hearts.Count<maxHealth)
        {
            int heartsNeeded = maxHealth - hearts.Count;

            for (int i = 0; i < heartsNeeded; i++)
            {
                GameObject heart = new GameObject("Heart");
                heart.transform.SetParent(transform);
                heart.transform.localScale = Vector3.one;
                heart.AddComponent<Image>();
                hearts.Add(heart.GetComponent<Image>());
            }
        }
        else if (hearts.Count>maxHealth)
        {
            for (int i = 0; i < hearts.Count - health / 2; i++)
            {
                hearts.RemoveAt(hearts.Count - 1);
                Destroy(hearts[^1].gameObject);
            }
        }
        
        for (var i = 0; i < hearts.Count; i++)
        {
            hearts[i].GetComponent<RectTransform>().anchoredPosition = Vector2.zero + new Vector2(0,-heartSpacing * i * (compressed?compression:1));
        }
    }
    
    public void SetHealth(int health)
    {
        int healthToSet = health / 2;
        for (int i = 0; i < hearts.Count; i++)
        {
            Image child = hearts[i];
            child.transform.DOKill();
            if (i <healthToSet)
            {
                child.sprite = fullHeart;
            }
            else if (i == healthToSet && health % 2 == 1)
            {
                child.sprite = halfHeart;
            }
            else
            {
                child.sprite = emptyHeart;
            }

            if (healthToSet ==i)
            {
                child.transform.DOShakeScale(0.5f);
            }
            
            if (!shake) return;
            
            child.transform.DOShakeRotation(0.5f, 50,20);

           if (health <= hearts.Count)//if remaining health is half of max or less, shake the hearts. Increase severity as we near 0.
            {
                child.transform.DOShakePosition(0.5f, shakeStrength * (1 - (float) health / hearts.Count+1)).SetLoops(-1, LoopType.Yoyo).SetDelay(0.25f*i);
            }
        }
    }
}
