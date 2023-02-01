using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceNode : Entity, IDamageable, IExperienceReward
{
    public ResourceDrop.Resource[] resourcePool;
    public Resource resourceNode;
    public uint resourceAmount = 10;
    public int experienceReward = 5;

    protected override void Start()
    {
        
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
    
    public void SetExperienceReward(int experienceReward)
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

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GiveResources();
        }
    }

    private void GiveResources()
    {
        for (int i = 0; i < resourceAmount; i++)
        {
            ResourceDrop.Resource resource = resourcePool[Random.Range(0, resourcePool.Length)];
            ResourceManager.instance.SpawnResourceDrop(transform.position, resource);
        }

        Player.instance.AddNodeHarvest(resourceNode);
        //TODO ResourceManager.instance.SpawnExperience(experienceReward);
        ResourceManager.instance.ReleaseResourceNode(this);
    }

    public int GetExperienceReward()
    {
        return experienceReward;
    }
    
    protected override void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Rb.AddForceAtPosition((col.transform.position - transform.position).normalized * -1000f, col.GetContact(0).point);
            TakeDamage(1);
        }
        base.OnCollisionEnter2D(col);
    }

    public enum Resource
    {
        Stone,
        Wood,
    }
}
