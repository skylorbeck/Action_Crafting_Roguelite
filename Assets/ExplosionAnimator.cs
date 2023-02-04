using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimator : SpriteAnimator
{
    protected void Awake()
    {
        
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
