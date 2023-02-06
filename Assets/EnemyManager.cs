using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    private Camera mainCamera;
    public static EnemyManager instance;
    private ObjectPool<Enemy> enemies;
    public List<Enemy> activeEnemies = new List<Enemy>();
    public List<Enemy> visibleEnemies = new List<Enemy>();
    [SerializeField] private int maxEnemies = 25;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private RoundObject round;
    [SerializeField] private bool spawnEnemies = false;
    [SerializeField] private float enemySpawnRate = 1;
    private float enemySpawnTimer = 0;
    
    [SerializeField] private Vector2Int enemySpawnRange = new Vector2Int(30, 30);//TODO Move this to a global settings class
    void Awake()
    {
        instance = this;
        mainCamera = Camera.main;
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

    private void FixedUpdate()
    {
        enemySpawnTimer += Time.fixedDeltaTime;
        if (enemySpawnTimer >= enemySpawnRate)
        {
            enemySpawnTimer = 0;
            CheckForRoomAndSpawnEnemy();
        }
    }

    private void CheckForRoomAndSpawnEnemy()
    {
        if (activeEnemies.Count < maxEnemies && spawnEnemies)
        {
            SpawnEnemy();
        }
    }
    
    public void SpawnEnemy()
    {
        var enemy = enemies.Get();
        enemy.SetPrefab(round.GetRandomEnemy());
        //place the enemy off the screen in a random direction
        Vector3 position = Vector3.zero;
        // float orthographicSize = mainCamera.orthographicSize+1;
        float orthographicSize = 11;//Used to use the cameras current size but you could change the difficulty by changing the zoom level, so now we take the max zoom level
        float aspect = mainCamera.aspect;
        do
        {
            int randomDirection = Random.Range(0, 4);
            switch (randomDirection)
            {
                case 0:
                    position = new Vector3(mainCamera.transform.position.x + orthographicSize * aspect,
                        Random.Range(-orthographicSize, orthographicSize), 0);
                    break;
                case 1:
                    position = new Vector3(mainCamera.transform.position.x - orthographicSize * aspect,
                        Random.Range(-orthographicSize, orthographicSize), 0);
                    break;
                case 2:
                    position = new Vector3(Random.Range(-orthographicSize * aspect, orthographicSize * aspect),
                        mainCamera.transform.position.y + orthographicSize, 0);
                    break;
                case 3:
                    position = new Vector3(Random.Range(-orthographicSize * aspect, orthographicSize * aspect),
                        mainCamera.transform.position.y - orthographicSize, 0);
                    break;
            }
        } while (position.x<-enemySpawnRange.x || position.x>enemySpawnRange.x || position.y<-enemySpawnRange.y || position.y>enemySpawnRange.y);

        enemy.ApplyMaxHealth();
        enemy.transform.position = position;
    }

    public void SpawnEnemy(Vector3 position)
    {
        var enemy = enemies.Get();
        enemy.transform.position = position;
    }
    
    public void ReleaseEnemy(Enemy enemy)
    {
        if (!enemy.gameObject.activeSelf) return;
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

    public void StartRound(RoundObject round)
    {
        this.round = round;
        enemySpawnRate = round.spawnRate;
        spawnEnemies = true;
    }

    public void SetSpawnEnemies(bool b)
    {
        spawnEnemies = b;
    }
}
