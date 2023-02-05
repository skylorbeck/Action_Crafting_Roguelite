using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceNode : Entity, IDamageable, IExperienceReward
{
    public Collider2D collider;
    public ResourceDrop.Resource[] resourcePool;
    public Resource resourceNode;
    public uint resourceAmount = 10;
    public uint experienceReward = 5;
    //TODO resource node size
    
    [SerializeField] float knockbackPlayer = 0.5f;
    [SerializeField] float knockbackWeapon = 0.5f;
    protected override void Start()
    {
        collider = GetComponent<Collider2D>();
        base.Start();
    }

    private void Awake()
    {
        Rb.freezeRotation = PlayerPrefs.GetInt("sillyMode", 0) == 0;
    }
    
    public void SetResourcePool(ResourceDrop.Resource[] resourcePool)
    {
        this.resourcePool = resourcePool;
    }
    
    public void SetResourceAmount(uint resourceAmount)
    {
        this.resourceAmount = resourceAmount;
    }
    
    public void SetExperienceReward(uint experienceReward)
    {
        this.experienceReward = experienceReward;
    }
    
    public void SetHealth(int health)
    {
        this.health = health;
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
    
    public void SetResource(ResourceNode newNode)
    {
        resourceNode = newNode.resourceNode;
        SetResourcePool(newNode.resourcePool);
        SetResourceAmount(newNode.resourceAmount);
        SetExperienceReward(newNode.experienceReward);
        SetHealth(newNode.health);
        SetSprite(newNode.sprites[0]);
    }

    protected override void Update()
    {
        base.Update();
    }

     protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public bool TakeDamage(int damage)
    {
        health -= damage;
        this.spriteRenderer.DOColor(Color.blue, 0.1f).OnComplete(() => this.spriteRenderer.DOColor(Color.white, 0.1f));
        this.transform.DOShakePosition(0.1f, 0.1f);
        if (health <= 0)
        {
            GiveResources();
            return true;
        }
        return false;
    }

    private void GiveResources()
    {
        var position = transform.position;

        int resourceAmount = Mathf.RoundToInt(this.resourceAmount + Player.instance.GetResourceBonus());

        for (int i = 0; i < resourceAmount ; i++)
        {
            ResourceDrop.Resource resource = resourcePool[Random.Range(0, resourcePool.Length)];
            ResourceManager.instance.SpawnResourceDrop(position, resource);
        }

        if (Player.instance.NodesExplode() && resourceNode == Resource.Stone)
        {
            ExplosionManager.instance.SpawnDamagingExplosion(position);
        }

        if (Player.instance.TreesBurn() && resourceNode == Resource.Wood)
        {
            ExplosionManager.instance.SpawnBurningExplosion(position);
        }
        
        Player.instance.AddNodeHarvest(resourceNode);
        ResourceManager.instance.SpawnExperienceOrb(position, experienceReward);
        ResourceManager.instance.ReleaseResourceNode(this);
    }

    public uint GetExperienceReward()
    {
        return experienceReward;
    }
    
    protected override void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Rb.AddForceAtPosition((col.transform.position - transform.position).normalized * -1000f*knockbackPlayer, col.GetContact(0).point);
            // TakeDamage(1);
        }
        //Moved to Projectile.cs
        /*if (col.gameObject.layer == LayerMask.NameToLayer("PlayerWeapons"))
        {
            Rb.AddForceAtPosition((col.transform.position - transform.position).normalized * -1000f * knockbackWeapon, col.GetContact(0).point);
            if (col.gameObject.GetComponent<Projectile>().targetResource == resourceNode)
            {
                TakeDamage(2);
            }
            else
            {
                TakeDamage(1);
            }
        }*/
        
        base.OnCollisionEnter2D(col);
    }

    public enum Resource
    {
        Stone,
        Wood,
    }
}
