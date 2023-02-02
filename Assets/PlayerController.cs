using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 5f;
    
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
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        transform.localScale = move.x switch
        {
            //flip sprite if moving right
            > 0 => new Vector3(-1, 1, 1),
            < 0 => new Vector3(1, 1, 1),
            _ => transform.localScale
        };
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        
    }
    
    public void OnFire(InputAction.CallbackContext context)
    {
        
    }

   

  }
