using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndependentSpriteAnimator : MonoBehaviour
{
    [SerializeField] protected Sprite[] sprites;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] private int deltaFrameDelay = 3;
    private int currentFrameDelay = 0;
    [SerializeField] private int index = 0;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        currentFrameDelay++;
        if (currentFrameDelay >= deltaFrameDelay)
        {
            currentFrameDelay = 0;
            index++;
            if (index >= sprites.Length)
            {
                index = 0;
            }
            spriteRenderer.sprite = sprites[index];
        }
    }
}
