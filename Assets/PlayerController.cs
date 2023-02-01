using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 5f;
    public float lookSpeed = 5f;
    public float fireRate = 0.5f;
    public float nextFire = 0f;
    
    Vector2 move;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = move * speed;
    }

    void FixedUpdate()
    {
        nextFire += Time.fixedDeltaTime;
        if (nextFire >= fireRate)
        {
            // Fire();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        // rb.velocity = move * speed;
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        
    }
    
    public void OnFire(InputAction.CallbackContext context)
    {
        
    }

   

  }
