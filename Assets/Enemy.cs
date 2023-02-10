using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Entity, IDamageable
{
    [SerializeField]protected float speed = 2.5f;
    [SerializeField]protected int power = 1;
    [SerializeField]protected uint goldReward = 1;
    [SerializeField]protected SpriteAnimator spriteAnimator;
    [SerializeField]protected int maxHealth = 2;
    [SerializeField]protected CircleCollider2D collider;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        ApplyMaxHealth();
    }
    
    public void ApplyMaxHealth(float multiplier = 1)
    {
        health = maxHealth = Mathf.RoundToInt(maxHealth * Player.instance.GetEnemyHealthMultiplier() * multiplier);
    }
    
    
    public void SetPower(int power)
    {
        this.power = power;
    }
    
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    
    public void SetGoldReward(uint goldReward)
    {
        this.goldReward = goldReward;
    }
    

    protected override void FixedUpdate()
    {
        var direction = (Player.instance.transform.position - transform.position).normalized;
        Rb.velocity = direction * (speed * Player.instance.GetEnemySpeedMultiplier());

        base.FixedUpdate();
    }

    public int GetPower()
    {
        return power;
    }
    
    public bool TakeDamage(int damage)
    {
        EnemyManager.instance.PlayHit(hitSound);
        health -= damage;
        this.spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() => this.spriteRenderer.DOColor(Color.white, 0.1f));
        if (health <= 0)
        {
            Kill();
            return true;
        }

        return false;
    }
    private void Kill()
    {
        var position = transform.position;
        ExplosionManager.instance.SpawnExplosion(position);
        ResourceManager.instance.SpawnCoin(position, goldReward);
        Player.instance.AddEnemyKill();
        EnemyManager.instance.ReleaseEnemy(this);
    }

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Player") || col.gameObject.layer == LayerMask.NameToLayer("PlayerWeapons"))
        {
            TakeDamage(1);
        }
        base.OnCollisionEnter2D(col);
    }
    
    public void OnBecameVisible()
    {
        EnemyManager.instance.visibleEnemies.Add(this);
    }
    
    public void OnBecameInvisible()
    {
        EnemyManager.instance.visibleEnemies.Remove(this);
    }

    public void SetPrefab(Enemy getRandomEnemy)
    {
        health = getRandomEnemy.health;
        maxHealth = getRandomEnemy.maxHealth;
        speed = getRandomEnemy.speed;
        power = getRandomEnemy.power;
        goldReward = getRandomEnemy.goldReward;
        sprites = getRandomEnemy.sprites;
        spriteAnimator.SetSprites(sprites);
        spriteRenderer.sprite = sprites[0];
        collider.offset = getRandomEnemy.collider.offset;
        collider.radius = getRandomEnemy.collider.radius;
    }
}
