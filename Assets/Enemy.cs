using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity, IDamageable
{
    [SerializeField]protected float speed = 2.5f;
    [SerializeField]protected int power = 10;
    [SerializeField]protected uint goldReward = 1;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        Rb.velocity = (Player.instance.transform.position - transform.position).normalized * speed;
        base.Update();
    }
    private void Awake()
    {
        Rb.freezeRotation = PlayerPrefs.GetInt("sillyMode", 0) == 0;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public int GetPower()
    {
        return power;
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Kill();
        }
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
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            TakeDamage(1);
        }
        base.OnCollisionEnter2D(col);
    }
}
