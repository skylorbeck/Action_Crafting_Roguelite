using System.Collections;
using System.Collections.Generic;
using DamageNumbersPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PopupManager : MonoBehaviour
{
    public static PopupManager instance;
    public DamageNumber defaultNumberPrefab;
    public DamageNumber criticalNumberPrefab;
    public DamageNumber healNumberPrefab;
    

    private void Awake()
    {
        instance = this;
    }
    
    public void SpawnDamageNumber(int damage, Vector3 position)
    {
        DamageNumber number = defaultNumberPrefab.Spawn(position, damage);
    }
    
    public void SpawnCriticalNumber(int damage, Vector3 position)
    {
        DamageNumber number = criticalNumberPrefab.Spawn(position, damage);
    }
    
    public void SpawnHealNumber(int heal, Vector3 position)
    {
        DamageNumber number = healNumberPrefab.Spawn(position, heal);
    }
}
