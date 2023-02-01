using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    private ObjectPool<Enemy> enemies;
    public List<Enemy> activeEnemies = new List<Enemy>();
    public Enemy enemyPrefab;
    void Awake()
    {
        instance = this;
        enemies = new ObjectPool<Enemy>(
            () =>
            {
                Enemy enemy = Instantiate(enemyPrefab, transform);
                return enemy;
            },
            enemy =>
            {
                enemy.gameObject.SetActive(true);
                activeEnemies.Add(enemy);
            },
            enemy =>
            {
                activeEnemies.Remove(enemy);
                enemy.gameObject.SetActive(false);
            }
        );
    }
    //TODO replace this entire thing with a wave manager that tells the manager what to spawn and when
    private void Start()
    {
        TimerManager.instance.onOneSecond += SpawnEnemy;
    }
    private void OnDestroy()
    {
        TimerManager.instance.onOneSecond -= SpawnEnemy;
    }

    public void SpawnEnemy()
    {
        var enemy = enemies.Get();
        Vector3 playerPos = Player.instance.transform.position;
        enemy.transform.position = new Vector3(Random.Range(playerPos.x - 10, playerPos.x + 10), Random.Range(playerPos.y - 10, playerPos.y + 10), 0);
    }

    public void SpawnEnemy(Vector3 position)
    {
        var enemy = enemies.Get();
        enemy.transform.position = position;
    }
    
    public void ReleaseEnemy(Enemy enemy)
    {
        enemies.Release(enemy);
    }
    
    public void ReleaseAllEnemies()
    {
        List<Enemy> enemiesToRelease = new List<Enemy>(activeEnemies);
        foreach (var enemy in enemiesToRelease)
        {
            enemies.Release(enemy);
        }
    }
}
