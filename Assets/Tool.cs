using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tool : MonoBehaviour
{
    [SerializeField] public ResourceNode.Resource targetResource = ResourceNode.Resource.Stone;
    [SerializeField] protected SpriteRenderer targetReticule;

    [SerializeField] private string toolName = "Tool";
    [SerializeField] private string description = "This is a tool";
    
    public float fireTimer = 0f;
    public float fireRate = 1f;
    
    [SerializeField] public ToolStats toolStats = new ToolStats();
    
    
    protected virtual void FixedUpdate()
    {
        if (fireRate==0) return;
        fireTimer += Time.fixedDeltaTime * Player.instance.GetAttackSpeedBonus();
        
        if (fireTimer >= fireRate)
        {
            StartCoroutine(Fire());
            fireTimer = 0f;
        }
    }

    public virtual IEnumerator Fire()
    {
        yield return null;
    }

    public virtual void RemoveTarget(Entity enemy)
    {
        
    }

    
}
