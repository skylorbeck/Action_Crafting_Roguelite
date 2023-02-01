using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity, IDamageable
{
    [SerializeField]protected float speed = 2.5f;
    [SerializeField]protected int power = 10;

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
        ExplosionManager.instance.SpawnExplosion(transform.position);
        Player.instance.AddEnemyKill();
        EnemyManager.instance.ReleaseEnemy(this);
    }
}
