using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimator : SpriteAnimator
{
    [SerializeField] protected Image image;
    protected override void NextSprite()
    {
        currentSpriteIndex++;
        if (currentSpriteIndex >= sprites.Length)
        {
            currentSpriteIndex = 0;
        }
        image.sprite = sprites[currentSpriteIndex];
    }
    
    protected override void PreviousSprite()
    {
        currentSpriteIndex--;
        if (currentSpriteIndex < 0)
        {
            currentSpriteIndex = sprites.Length - 1;
        }
        image.sprite = sprites[currentSpriteIndex];
    }
}
