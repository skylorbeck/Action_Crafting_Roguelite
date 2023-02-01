using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    public static Player instance;
    
    [SerializeReference]RunStats runStats;
    
    [SerializeField]protected int maxHealth = 100;
    [SerializeField]protected uint experience = 0;
    [SerializeField]protected uint experienceToNextLevel = 100;
    [SerializeField]protected uint level = 1;

    [SerializeField]protected float invincibleTime = 1f;
    [SerializeField]protected float invincibleTimer = 0f;

    protected override void Start()
    {
        instance = this;
        base.Start();
    }

    private void Awake()
    {
        health = maxHealth;
        Rb.freezeRotation = PlayerPrefs.GetInt("sillyMode", 0) == 0;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (invincibleTimer >0)
        {
            invincibleTimer -= Time.deltaTime;
        }
        base.FixedUpdate();
    }

    public void TakeDamage(int damage)
    {
        spriteRenderer.DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
        transform.DOShakeScale(1f, 0.5f);
        health -= damage;
        if (health <= 0)
        {
            Kill();
        }
    }

    private void Kill()
    {
        ExplosionManager.instance.SpawnExplosion(transform.position);
        EnemyManager.instance.ReleaseAllEnemies();
        gameObject.SetActive(false); //TODO replace this
    }


    protected override void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy") && invincibleTimer <= 0)
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            TakeDamage(enemy.GetPower());
            invincibleTimer = invincibleTime;
        }
        base.OnCollisionEnter2D(col);
    }

    protected override void OnCollisionStay2D(Collision2D col)
    {
        OnCollisionEnter2D(col);
        base.OnCollisionStay2D(col);
    }

    public void AddEnemyKill()
    {
        runStats.enemiesKilled++;
    }

    public void AddResource(ResourceDrop.Resource resource, uint amount)
    {
        runStats.resourcesCollected += amount;
        switch (resource)
        {
            case ResourceDrop.Resource.Stone:
                runStats.stoneCollected += amount;
                break;
            case ResourceDrop.Resource.Wood:
                runStats.woodCollected += amount;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resource), resource, null);
        }
    }

    public void AddNodeHarvest(ResourceNode.Resource resource)
    {
        switch (resource)
        {
            case ResourceNode.Resource.Stone:
                runStats.stoneNodesHarvested++;
                break;
            case ResourceNode.Resource.Wood:
                runStats.woodNodesHarvested++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resource), resource, null);
        }
    }
}

