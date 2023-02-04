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