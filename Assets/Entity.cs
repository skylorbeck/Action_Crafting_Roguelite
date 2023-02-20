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
    [SerializeField] protected bool magnetToPlayer = false;
    public bool isAlive => health > 0;
    protected virtual IEnumerator Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<SpriteAnimator>()?.SetSprites(sprites);
        yield return null;
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
        if (magnetToPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, Player.instance.transform.position, Time.fixedDeltaTime * 10);
        }
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

    public void Vacuum()
    {
        magnetToPlayer = true;
    }
}