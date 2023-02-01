using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimator : SpriteAnimator
{
    [SerializeField]float frameTime = 0.1f;
    float timer = 0;
    protected void Awake()
    {
        
    }

    public void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= frameTime)
        {
            timer = 0;
            NextSprite();
        }
    }

    protected override void NextSprite()
    {
        currentSpriteIndex++;
        if (currentSpriteIndex >= sprites.Length)
        {
            // this.gameObject.SetActive(false);
            currentSpriteIndex = 0;
            ExplosionManager.instance.ReleaseExplosion(this);
        }
        spriteRenderer.sprite = sprites[currentSpriteIndex];
    }
}
