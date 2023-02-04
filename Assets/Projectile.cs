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
    
    public bool childProjectile = false;
    public float childProjectileTimer = 0f;
    public float childProjectileDuration = 0.5f;

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
        ((ThrowingTool)parent).ReleaseProjectile(Rb);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        int damage = Player.instance.GetDamage();
        bool crit = Random.Range(0f, 1) < Player.instance.GetCritChance();
        if (col.gameObject.layer == LayerMask.NameToLayer("ResourceNode"))
        {
            ResourceNode resourceNode = col.gameObject.GetComponent<ResourceNode>();
            if (resourceNode.resourceNode == targetResource)
            {
                damage *= 2;
            }
            if (crit)
            {
                damage = (int)(damage * Player.instance.GetCritDamageBonus());
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
                damage = (int)(damage * Player.instance.GetCritDamageBonus());
                PopupManager.instance.SpawnCriticalNumber(damage, enemy.transform.position);
            }
            else
            {
                PopupManager.instance.SpawnDamageNumber(damage, enemy.transform.position);
            }
            if (enemy.TakeDamage(damage))
            {
                if (Player.instance.GetPerkStatModifiers().enemiesSpawnPick && !childProjectile)
                {
                    Projectile child = ((ThrowingTool)parent).GetProjectile();
                    var transform1 = transform;
                    var position = transform1.position;
                    child.transform.position = position;
                    child.targetResource = targetResource;
                    child.childProjectile = true;

                    Vector3 direction = (enemy.transform.position+(Vector3)Random.insideUnitCircle - transform.position).normalized;
                    child.Rb.AddForce(direction * Rb.velocity.sqrMagnitude *5f);
                    child.Rb.AddTorque(Rb.angularVelocity);

                }
                RemoveFromParent(enemy);
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