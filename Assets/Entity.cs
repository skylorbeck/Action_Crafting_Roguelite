using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected Sprite[] sprites;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    public Rigidbody2D Rb;
    [SerializeField] protected AudioClip hitSound;
    [SerializeField] protected int health = 1;

    protected virtual void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<SpriteAnimator>()?.SetSprites(sprites);
    }

    protected virtual void Awake()
    {
        Rb.freezeRotation = PlayerPrefs.GetInt("sillyMode", 0) == 0;
        
    }

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {
    }

    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
    }

    protected virtual void OnCollisionExit2D(Collision2D other)
    {
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
    }

    
}