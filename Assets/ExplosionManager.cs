using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ExplosionManager : MonoBehaviour
{
    public static ExplosionManager instance;
    private ObjectPool<ExplosionAnimator> explosions;
    public List<ExplosionAnimator> activeExplosions = new List<ExplosionAnimator>();
    public ExplosionAnimator explosionPrefab;

    private void Awake()
    {
        instance = this;
        explosions = new ObjectPool<ExplosionAnimator>(
            () => Instantiate(explosionPrefab, transform),
            explosion =>
            {
                explosion.gameObject.SetActive(true);
                activeExplosions.Add(explosion);
            },
            explosion =>
            {
                activeExplosions.Remove(explosion);
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
        explosions.Release(explosion);
    }
}