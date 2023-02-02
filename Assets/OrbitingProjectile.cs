using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingProjectile : Projectile
{
    
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        
    }
}
