using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;
using Task = System.Threading.Tasks.Task;

public class ThrowingTool : Tool
{
    [SerializeField] private GameObject throwablePrefab;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwTorque = 1;
    [SerializeField] private int projectileCount = 1;
    [SerializeField] private int splitPickCount = 3;
    [SerializeField] private float splitPickAngle = 30;
    [SerializeField] private float splitPickSize = 0.75f;
    [SerializeField] private List<GameObject> primaryTargets = new List<GameObject>();

    ObjectPool<Projectile> throwables;

    public void Awake()
    {
        throwables = new ObjectPool<Projectile>(
            () =>
            {
                Projectile throwable = Instantiate(throwablePrefab, transform.position, transform.rotation)
                    .GetComponent<Projectile>();
                throwable.GetComponent<Projectile>().parent = this;
                return throwable;
            },
            throwable =>
            {
                throwable.gameObject.SetActive(true);
                var transform1 = throwable.transform;
                transform1.localScale =
                    Vector3.one * Player.instance.GetProjectileSizeBonus();
                var transform2 = transform;
                transform1.position = transform2.position;
                transform1.rotation = transform2.rotation;
                throwable.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            },
            throwable =>
            {
                throwable.GetComponent<TrailRenderer>().Clear();
                throwable.gameObject.SetActive(false);
            }
        );
    }

    public override async void Fire()
    {
        Fire(true);
    }

    private async void Fire(bool fromPlayer)
    {
        Transform playerTransform = Player.instance.transform;
        Vector3 playerPosition;

        int projectiles = this.projectileCount + Player.instance.GetExtraProjectiles();

        List<Projectile> throwablesToRelease = new List<Projectile>();
        for (int i = 0; i < projectiles; i++)
        {
            playerPosition = playerTransform.position;
            if (fromPlayer)
            {
                this.transform.position = playerPosition;
            }

            primaryTargets.Sort((a, b) => Vector3.Distance(playerPosition, a.transform.position)
                .CompareTo(Vector3.Distance(playerTransform.position, b.transform.position)));

            int amountOfPicks = 1;

            if (Player.instance.SplitPicks())
            {
                amountOfPicks = splitPickCount;
            }
            Vector3 target = primaryTargets.Count > 0
                ? primaryTargets[0].transform.position
                : Random.insideUnitCircle + (Vector2)playerTransform.position;
            for (int j = 0; j < amountOfPicks; j++)
            {
                Projectile throwable = throwables.Get();
                Vector3 direction = (target - playerTransform.position).normalized;
                if (j>0)
                {
                    direction = Quaternion.Euler(0, 0, splitPickAngle / amountOfPicks * j) * direction;
                }

                if (Player.instance.SplitPicks())
                {
                    throwable.transform.localScale *= splitPickSize;
                    throwable.damageScale *= splitPickSize;
                }
                Rigidbody2D rb = throwable.GetComponent<Rigidbody2D>();
                rb.AddForce(direction * (throwForce * Player.instance.GetProjectileSpeedBonus()),
                    ForceMode2D.Impulse);
                rb.AddTorque(throwTorque, ForceMode2D.Impulse);
                throwablesToRelease.Add(throwable);
            }

            await Task.Delay(100);
        }

        await Task.Delay(1000);

        foreach (var t in throwablesToRelease)
        {
            throwables.Release(t);
            await Task.Delay(100);
        }
    }

    public void FireFromPosition(Vector3 position)
    {
        this.transform.position = position;
        Fire(false);
    }

    public Projectile GetProjectile()
    {
        return throwables.Get();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("ResourceNode"))
        {
            primaryTargets.Add(col.gameObject);
            /*if (col.GetComponent<ResourceNode>().resourceNode == targetResource)
            {
                primaryTargets.Add(col.gameObject);
            }
            else
            {
                secondaryTargets.Add(col.gameObject);
            }*/

        }

        // if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        // {
        //     secondaryTargets.Add(col.gameObject);
        // }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("ResourceNode"))
        {
            primaryTargets.Remove(col.gameObject);
        }
    }

    public override void RemoveTarget(Entity target)
    {
        primaryTargets.Remove(target.gameObject);
    }

    public void ReleaseProjectile(Projectile projectile)
    {

        throwables.Release(projectile);
    }
}
