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
    
    public DamageNumber coinNumberUIPrefab;
    public RectTransform goldIcon;

    public DamageNumber ExpNumberUIPrefab;
    public RectTransform ExpIcon;
    
    public DamageNumber stoneNumberUIPrefab;
    public RectTransform stoneIcon;
    
    public DamageNumber woodNumberUIPrefab;
    public RectTransform woodIcon;
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
    
    public void SpawnCoinNumber(int coin)
    {
        DamageNumber number = coinNumberUIPrefab.Spawn(Vector3.zero, "+");
        number.number = coin;
        number.SetAnchoredPosition(goldIcon,Vector2.zero);
    }
    
    public void SpawnExpNumber(int exp)
    {
        DamageNumber number = ExpNumberUIPrefab.Spawn(Vector3.zero, "+");
        number.number = exp;
        number.SetAnchoredPosition(ExpIcon,Vector2.zero);
    }
    
    public void SpawnStoneNumber(int stone)
    {
        DamageNumber number = stoneNumberUIPrefab.Spawn(Vector3.zero, "+");
        number.number = stone;
        number.SetAnchoredPosition(stoneIcon,Vector2.zero);
    }
    
    public void SpawnWoodNumber(int wood)
    {
        DamageNumber number = woodNumberUIPrefab.Spawn(Vector3.zero, "+");
        number.number = wood;
        number.SetAnchoredPosition(woodIcon,Vector2.zero);
    }
}
