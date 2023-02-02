using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    [SerializeField] protected ResourceNode.Resource targetResource = ResourceNode.Resource.Stone;

    public float fireTimer = 0f;
    public float fireRate = 1f;

    protected async void FixedUpdate()
    {
        if (fireRate==0) return;
        fireTimer += Time.fixedDeltaTime;
        
        if (fireTimer >= fireRate)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    public virtual void Fire()
    {
        //do stuff here
    }
}
