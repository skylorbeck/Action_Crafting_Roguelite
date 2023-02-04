using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
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

    private ObjectPool<Projectile> swingables;

    public List<Transform> activeSwingables = new List<Transform>();

    private void Awake()
    {
        swingables = new ObjectPool<Projectile>(
            () =>
            {
                Projectile swingable = Instantiate(swingablePrefab, transform.position, Quaternion.identity)
                    .GetComponent<Projectile>();
                swingable.spriteRenderer.DOFade(0, 0);
                swingable.gameObject.SetActive(false);
                return swingable;
            },
            swingable =>
            {
                swingable.gameObject.SetActive(true);
                var transform1 = swingable.transform;
                transform1.localScale =
                    Vector3.one * Player.instance.GetProjectileSizeBonus();
                transform1.position = transform.position;
                transform1.rotation = transform.rotation;
                swingable.Rb.velocity = Vector2.zero;
            },
            swingable =>
            {
                activeSwingables.Remove(swingable.transform);
                swingable.trailRenderer.Clear();
                swingable.gameObject.SetActive(false);
            }
        );

        Fire();
    }

    public override async void Fire()
    {
        List<Projectile> swingablesToDestroy = new List<Projectile>();
        for (int i = 0; i < swingableCount+Player.instance.GetExtraProjectiles(); i++)
        {
            var swingable = swingables.Get();
            swingable.transform.parent = transform;
            float angle = i / (float)(swingableCount+Player.instance.GetExtraProjectiles());
            float x = Mathf.Cos(angle * Mathf.PI * 2f) * swingDistance * Player.instance.GetAoERadius();
            float y = Mathf.Sin(angle * Mathf.PI * 2f) * swingDistance * Player.instance.GetAoERadius();
            swingable.transform.localPosition = new Vector3(x, y, 0);
            Transform transform1;
            
            (transform1 = swingable.transform).Rotate(Vector3.forward, angle * 360f);
            activeSwingables.Add(transform1);
            swingablesToDestroy.Add(swingable);
            swingable.spriteRenderer.DOFade(1, 0.5f);
        }

        await Task.Delay(TimeSpan.FromSeconds(swingTime));

        foreach (Projectile swingable in swingablesToDestroy)
        {
            swingable.spriteRenderer.DOFade(0, 0.5f).onComplete += () =>
                swingables.Release(swingable);
        }

        base.Fire();
    }

    public new void FixedUpdate()
    {
        transform.position = Player.instance.transform.position;
        foreach (var swingable in activeSwingables)
        {
            var position = transform.position;
            swingable.transform.position = position + (swingable.position - position).normalized * (swingDistance * Player.instance.GetAoERadius());
            swingable.transform.RotateAround(position, Vector3.forward, swingSpeed * Time.fixedDeltaTime * Player.instance.GetProjectileSpeedBonus());
            swingable.transform.Rotate(Vector3.forward, swingSpeed * 2.5f * Time.fixedDeltaTime);
        }

        base.FixedUpdate();
    }
}