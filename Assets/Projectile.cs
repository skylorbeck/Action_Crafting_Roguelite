using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ResourceNode.Resource targetResource = ResourceNode.Resource.Stone;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D Rb;
    public TrailRenderer trailRenderer;
    public Tool parent;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("ResourceNode"))
        {
            ResourceNode resourceNode = col.gameObject.GetComponent<ResourceNode>();
            if (resourceNode.resourceNode == targetResource)
            {
                resourceNode.TakeDamage(2);//TODO perks modify this
            }
            else
            {
                resourceNode.TakeDamage(1);//TODO perks modify this
            }
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            if (enemy.TakeDamage(1))//TODO perks modify this
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