using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tool : MonoBehaviour
{
    [SerializeField] public ResourceNode.Resource targetResource = ResourceNode.Resource.Stone;

    [SerializeField] protected SpriteRenderer targetReticule;

    public float fireTimer = 0f;
    public float fireRate = 1f;

    public float damageScale = 1f;//TODO add a bunch of other parameters to customize tools with. Consider making this a class with overrides for each tool type

    
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
