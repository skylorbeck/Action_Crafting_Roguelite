using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Round", menuName = "RoundObject", order = 1)]
public class RoundObject : ScriptableObject
{
    [SerializeField] Enemy[] enemies;
    
    public Enemy[] GetEnemies()
    {
        return enemies;
    }
    
    public Enemy GetRandomEnemy()
    {
        return enemies[Random.Range(0, enemies.Length)];
    }
    
}
