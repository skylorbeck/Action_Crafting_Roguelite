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
    public float combineAxesSizeBonus = .5f;
    public float splitAxesSizeBonus = 0.75f;
    
    private ObjectPool<Projectile> swingables;

    public List<Transform> activeSwingables = new List<Transform>();

    private void Awake()
    {
        swingables = new ObjectPool<Projectile>(
            () =>
            {
                Projectile swingable = Instantiate(swingablePrefab, transform.position, Quaternion.identity)
                    .GetComponent<Projectile>();
                swingable.spriteRenderer.sprite = Player.instance.toolRegistry.axeImages[toolStats.toolTier];
                swingable.spriteRenderer.DOFade(0, 0);
                swingable.gameObject.SetActive(false);
                return swingable;
            },
            swingable =>
            {
                swingable.gameObject.SetActive(true);
                var transform1 = swingable.transform;
                transform1.localScale =
                    Vector3.one * swingable.projectileScale * Player.instance.GetProjectileSizeBonus();
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

        StartCoroutine(DelayFire());
    }

    public IEnumerator DelayFire()
    {
        yield return new WaitForFixedUpdate();
        StartCoroutine(Fire());
    }

    public override IEnumerator Fire()
    {
        List<Projectile> swingablesToDestroy = new List<Projectile>();
        int projectiles = this.swingableCount + Player.instance.GetExtraProjectiles();
        if (Player.instance.SplitAxes())
        {
            projectiles *= 2;
        }
       
        for (int i = 0; i < projectiles; i++)
        {
            var swingable = swingables.Get();
            swingable.transform.parent = transform;
            swingable.damageScale = toolStats.damageScale;

            if (Player.instance.SplitAxes())
            {
                swingable.damageScale *= splitAxesSizeBonus;
                swingable.transform.localScale *= splitAxesSizeBonus;
            }
            if (Player.instance.CombineAxes())
            {
                swingable.damageScale *= projectiles * combineAxesSizeBonus;
                swingable.transform.localScale *= (projectiles * combineAxesSizeBonus);
            }
            float angle = i / (float)(projectiles);
            float x = Mathf.Cos(angle * Mathf.PI * 2f) * swingDistance * Player.instance.GetAoERadius();
            float y = Mathf.Sin(angle * Mathf.PI * 2f) * swingDistance * Player.instance.GetAoERadius();
            swingable.transform.localPosition = new Vector3(x, y, 0);
            Transform transform1;
            
            (transform1 = swingable.transform).Rotate(Vector3.forward, angle * 360f);
            activeSwingables.Add(transform1);
            swingablesToDestroy.Add(swingable);
            swingable.spriteRenderer.DOFade(1, 0.5f);
            if (Player.instance.CombineAxes())
            {
                break;
            }
        }

        yield return new WaitForSeconds(swingTime+toolStats.projectileLifeTimeBonus);

        foreach (Projectile swingable in swingablesToDestroy)
        {
            swingable.spriteRenderer.DOFade(0, 0.5f).onComplete += () =>
                swingables.Release(swingable);
        }

        yield return base.Fire();
    }

    protected override void FixedUpdate()
    {
        transform.position = Player.instance.transform.position;
        foreach (var swingable in activeSwingables)
        {
            var position = transform.position;
            swingable.transform.position = position + (swingable.position - position).normalized * (swingDistance * Player.instance.GetAoERadius() *(Player.instance.CombineAxes()?Player.instance.GetProjectileSizeBonus():1));
            swingable.transform.RotateAround(position, Vector3.forward, swingSpeed * Time.fixedDeltaTime * Player.instance.GetProjectileSpeedBonus());
            swingable.transform.Rotate(Vector3.forward, swingSpeed * 2.5f * Time.fixedDeltaTime* Player.instance.GetProjectileSpeedBonus());
        }

        base.FixedUpdate();
    }
}