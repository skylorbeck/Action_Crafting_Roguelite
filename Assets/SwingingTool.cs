using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class SwingingTool : Tool
{
    public Rigidbody2D swingablePrefab;
    public int swingableCount = 3;
    public float swingSpeed = 1f;
    public float swingDistance = 1f;
    public float swingTime = 5f;

    private ObjectPool<GameObject> swingables;

    public List<Transform> activeSwingables = new List<Transform>();

    private void Awake()
    {
        swingables = new ObjectPool<GameObject>(
            () =>
            {
                var swingable = Instantiate(swingablePrefab, transform.position, Quaternion.identity).gameObject;
                swingable.SetActive(false);
                return swingable;
            },
            swingable =>
            {
                swingable.SetActive(true);
                swingable.transform.localScale = Vector3.one *2f;//TODO stats modify this
                swingable.transform.position = transform.position;
                swingable.transform.rotation = transform.rotation;
                swingable.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            },
            swingable =>
            {
                activeSwingables.Remove(swingable.transform);
                swingable.GetComponent<TrailRenderer>().Clear();
                swingable.SetActive(false);
            }
        );
        
        
    }

    public override async void Fire()
    {
        List<GameObject> swingablesToDestroy = new List<GameObject>();
        for (int i = 0; i < swingableCount; i++)
        {
            var swingable = swingables.Get();
            swingable.transform.parent = transform;
            float angle = i / (float)swingableCount;
            float x = Mathf.Cos(angle * Mathf.PI * 2f) * swingDistance;
            float y = Mathf.Sin(angle * Mathf.PI * 2f) * swingDistance;
            swingable.transform.localPosition = new Vector3(x, y, 0);
            swingable.transform.Rotate(Vector3.forward, angle * 360f);
            activeSwingables.Add(swingable.transform);
            swingablesToDestroy.Add(swingable);
        }

        await Task.Delay(TimeSpan.FromSeconds(swingTime));
        
        foreach (GameObject swingable in swingablesToDestroy)
        {
            swingables.Release(swingable);
        }
        
        base.Fire();
    }

    public new void FixedUpdate()
    {
        transform.position = Player.instance.transform.position;
        foreach (var swingable in activeSwingables)
        {
             swingable.transform.position = transform.position + (swingable.position - transform.position).normalized * swingDistance;
             swingable.transform.RotateAround(transform.position, Vector3.forward, swingSpeed * Time.fixedDeltaTime);
             swingable.transform.Rotate(Vector3.forward, swingSpeed * 2.5f * Time.fixedDeltaTime);
        }
        base.FixedUpdate();
    }
}
