using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AoESpriteChanger : MonoBehaviour
{
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Sprite[] sprites;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            rb.AddTorque(100 * Random.Range(-0.5f, 0.5f));
            spriteRenderer.sprite = sprites[1];
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            spriteRenderer.sprite = sprites[0];
        }
    }
}
