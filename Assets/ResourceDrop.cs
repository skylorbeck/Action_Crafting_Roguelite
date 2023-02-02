using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDrop : Entity
{
    [SerializeField] Resource resource;
    public Collider2D collider;

    [SerializeField] uint amount;

    public void SetResource(Resource resource)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = ResourceManager.instance.resourceDropRegistry.GetResourceDropPrefab(resource).sprites[0];
        this.resource = resource;
    }

    public void SetAmount(uint amount)
    {
        this.amount = amount;
    }

    public Resource GetResource()
    {
        return resource;
    }

    public uint GetAmount()
    {
        return amount;
    }

    private void Awake()
    {
        Rb.freezeRotation = PlayerPrefs.GetInt("sillyMode", 0) == 0;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        collider.enabled = false;
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player.instance.AddResource(resource, amount);
            ResourceManager.instance.ReleaseResourceDrop(this);
        }
        base.OnTriggerEnter2D(col);
    }

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        
        base.OnCollisionEnter2D(col);
    }
    
    public enum Resource
    {
        Wood,
        Stone,
        Iron,
        Gold,
        Diamond
    }
}