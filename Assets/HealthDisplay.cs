using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public static HealthDisplay instance;
    
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    
    public List<Image> hearts = new List<Image>();


    public float shakeStrength = 2f;
    private void Awake()
    {
        instance = this;
    }

    public void SetMaxHealth(int health)
    {
        int maxHealth = (int) Math.Round(health / 2f, MidpointRounding.AwayFromZero);
        int heartsNeeded = maxHealth - hearts.Count;
        if (hearts.Count<heartsNeeded)
        {
            for (int i = 0; i < heartsNeeded; i++)
            {
                GameObject heart = new GameObject("Heart");
                heart.transform.SetParent(transform);
                heart.transform.localScale = Vector3.one;
                heart.transform.localPosition = Vector3.zero;
                heart.AddComponent<Image>();
                hearts.Add(heart.GetComponent<Image>());
            }
        }
        else if (heartsNeeded<0)
        {
            for (int i = 0; i < hearts.Count - health / 2; i++)
            {
                hearts.RemoveAt(hearts.Count - 1);
                Destroy(hearts[^1].gameObject);
            }
        }
    }
    
    public void SetHealth(int health)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
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
            child.transform.DOShakeRotation(0.5f, 50,20);

            if (healthToSet ==i)
            {
                // child.transform.DOPunchScale(Vector3.one * 0.5f, 0.5f, 10, 1);
                child.transform.DOShakeScale(0.5f);
            }

            if (health <= hearts.Count)//if remaining health is half of max or less, shake the hearts. Increase severity as we near 0.
            {
                child.transform.DOShakePosition(0.5f, shakeStrength * (1 - (float) health / hearts.Count+1)).SetLoops(-1, LoopType.Yoyo).SetDelay(0.25f*i);
            }
            
        }
    }
}
