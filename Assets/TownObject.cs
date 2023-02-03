using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TownObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float maxDistance = .1f;
    [SerializeField] private float rotationDistance = 0.1f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private int knockbackPlayer = 100;
    [SerializeField] private float randomRange = 1f;

    private void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    { 
        if (Vector3.Distance(transform.position, targetPosition) > maxDistance)
        {
            rb.velocity = (targetPosition - transform.position).normalized * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (Mathf.Abs(rb.rotation) > rotationDistance)
        {
            rb.AddTorque(-rb.rotation * rotationSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            rb.AddTorque(knockbackPlayer *Random.Range(-randomRange, randomRange));
        }
    }
}
