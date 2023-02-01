using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] protected Sprite[] sprites;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected int currentSpriteIndex = 0;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual async void OnEnable()
    {
        await Task.Delay(1);
        TimerManager.instance.onOneSecond += NextSprite;
    }

    protected virtual void NextSprite()
    {
           currentSpriteIndex++;
            if (currentSpriteIndex >= sprites.Length)
            {
                currentSpriteIndex = 0;
            }
            spriteRenderer.sprite = sprites[currentSpriteIndex];
    }

    private void PreviousSprite()
    {
        currentSpriteIndex--;
        if (currentSpriteIndex < 0)
        {
            currentSpriteIndex = sprites.Length - 1;
        }
        spriteRenderer.sprite = sprites[currentSpriteIndex];
    }

    public void SetSprites(Sprite[] newSprites)
    {
        sprites = newSprites;
    }
    
    private void OnDisable()
    {
        TimerManager.instance.onOneSecond -= NextSprite;
    }
}