using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour
{
    public ResourceNode.Resource targetResource = ResourceNode.Resource.Stone;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D Rb;
    public TrailRenderer trailRenderer;
    public Tool parent;
    public float damageScale = 1f;
    
    public bool continuousDamage = false;
    public float continuousDamageTimer = 0f;
    public float continuousDamageInterval = 0.5f;
    
    public bool childProjectile = false;//TODO Perk that allows child projectiles to spawn more child projectiles
    public float childProjectileTimer = 0f;
    public float childProjectileDuration = 0.5f;
    public float childProjectileSize = 0.5f;
    public float childProjectileDamage = 0.5f;

    public void FixedUpdate()
    {
        if (childProjectile)
        {
            childProjectileTimer += Time.fixedDeltaTime;
            if (childProjectileTimer >= childProjectileDuration)
            {
                ReleaseChildProjectile();
            }
        }
        
    }

    private void ReleaseChildProjectile()
    {
        childProjectileTimer = 0f;
        childProjectile = false;
        ((ThrowingTool)parent).ReleaseProjectile(this);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        int damage = Player.instance.GetDamage();
        bool crit = Random.Range(0f, 1) < Player.instance.GetCritChance();
        damage = Mathf.RoundToInt(damage * damageScale);
        if (childProjectile)
        {
            damage = Mathf.RoundToInt(damage * childProjectileDamage);
        }
        
        if (col.gameObject.layer == LayerMask.NameToLayer("ResourceNode"))
        {
            ResourceNode resourceNode = col.gameObject.GetComponent<ResourceNode>();
            if (resourceNode.resourceNode == targetResource)
            {
                damage *= 2;
            }
            if (crit)
            {
                damage = Mathf.RoundToInt(damage * Player.instance.GetCritDamageBonus());
                PopupManager.instance.SpawnCriticalNumber(damage, resourceNode.transform.position);
            }
            else
            {
                PopupManager.instance.SpawnDamageNumber(damage, resourceNode.transform.position);
            }
            
            resourceNode.TakeDamage(damage);
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            if (crit)
            {
                damage = Mathf.RoundToInt(damage * Player.instance.GetCritDamageBonus());
                PopupManager.instance.SpawnCriticalNumber(damage, enemy.transform.position);
            }
            else
            {
                PopupManager.instance.SpawnDamageNumber(damage, enemy.transform.position);
            }
            if (enemy.TakeDamage(damage))
            {
                if (Player.instance.PicksSpawnOnDeath() && !childProjectile)
                {
                    Projectile child = ((ThrowingTool)parent).GetProjectile();
                    var transform1 = transform;
                    var position = transform1.position;
                    child.transform.position = position;
                    child.targetResource = targetResource;
                    child.childProjectile = true;
                    child.transform.localScale *= childProjectileSize;

                    Vector3 direction = (enemy.transform.position+(Vector3)Random.insideUnitCircle - transform.position).normalized;
                    child.Rb.AddForce(direction * Rb.velocity.sqrMagnitude *5f);
                    child.Rb.AddTorque(Rb.angularVelocity);

                }
                RemoveFromParent(enemy);
            }
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (continuousDamage)
        {
            continuousDamageTimer += Time.fixedDeltaTime;
            if (continuousDamageTimer >= continuousDamageInterval)
            {
                continuousDamageTimer = 0f;
                OnTriggerEnter2D(other);
            }
        }
    }

    protected void RemoveFromParent(Entity target)
    {
        if (parent != null)
        {
            parent.RemoveTarget(target);
        }
    }
}