using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ExplosionManager : MonoBehaviour
{
    public static ExplosionManager instance;
    private ObjectPool<ExplosionAnimator> explosions;
    private ObjectPool<ExplosionAnimator> damagingExplosions;
    private ObjectPool<ExplosionAnimator> burningExplosions;
    public ExplosionAnimator explosionPrefab;
    public ExplosionAnimator damagingExplosionPrefab;
    public ExplosionAnimator burningExplosionPrefab;

    private void Awake()
    {
        instance = this;
        explosions = new ObjectPool<ExplosionAnimator>(
            () => Instantiate(explosionPrefab, transform),
            explosion =>
            {
                explosion.gameObject.SetActive(true);
            },
            explosion =>
            {
                explosion.gameObject.SetActive(false);
            }
        );
        damagingExplosions = new ObjectPool<ExplosionAnimator>(
            () => Instantiate(damagingExplosionPrefab, transform),
            explosion =>
            {
                explosion.gameObject.SetActive(true);
            },
            explosion =>
            {
                explosion.gameObject.SetActive(false);
            }
        );
        burningExplosions = new ObjectPool<ExplosionAnimator>(
            () => Instantiate(burningExplosionPrefab, transform),
            explosion =>
            {
                explosion.gameObject.SetActive(true);
            },
            explosion =>
            {
                explosion.gameObject.SetActive(false);
            }
        );
    }

    public void SpawnExplosion(Vector3 position)
    {
        var explosion = explosions.Get();
        explosion.transform.position = position;
    }
    
    public void ReleaseExplosion(ExplosionAnimator explosion)
    {
        if (explosion.gameObject.layer == LayerMask.NameToLayer("PlayerWeapons"))
        {
            damagingExplosions.Release(explosion);
            return;
        }
        explosions.Release(explosion);
    }
    
    public void SpawnDamagingExplosion(Vector3 position)
    {
        var explosion = damagingExplosions.Get();
        Transform transform1;
        (transform1 = explosion.transform).localScale = Vector3.one * 2 * Player.instance.GetAoERadius();
        transform1.position = position;
    }
    
    public void SpawnBurningExplosion(Vector3 position)
    {
        var explosion = burningExplosions.Get();
        Transform transform1;
        (transform1 = explosion.transform).localScale = Vector3.one * Player.instance.GetAoERadius();
        transform1.position = position;
    }
}