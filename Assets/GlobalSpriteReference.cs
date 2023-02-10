using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GlobalSpriteReference : MonoBehaviour
{
    public static GlobalSpriteReference instance;
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    [SerializeField] private Sprite woodDrop;
    public Sprite GetWoodDrop()
    {
        return woodDrop;
    }
    [SerializeField] private Sprite stoneDrop;
    public Sprite GetStoneDrop()
    {
        return stoneDrop;
    }
    [SerializeField] private Sprite goldCoinDrop;
    public Sprite GetGoldCoinDrop()
    {
        return goldCoinDrop;
    }

}
