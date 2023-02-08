using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [SerializeField] private RunStats metaStats; 
    [SerializeField] private TownStats townStats; 
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void AddRunToMetaStats(RunStats runStats)
    {
        metaStats.AddRunStats(runStats);
    }
    
    public void SetMetaStats(RunStats oldMetaStats)
    {
        metaStats = oldMetaStats;
    }
    
    public RunStats GetMetaStats()
    {
        return metaStats;
    }
    
    public void ResetMetaStats()
    {
        metaStats.Reset();
    }
    
    public void SetTownStats(TownStats oldTownStats)
    {
        townStats = oldTownStats;
    }
    
    public TownStats GetTownStats()
    {
        return townStats;
    }
    
    public void ResetTownStats()
    {
        townStats.Reset();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(metaStats);
        File.WriteAllText(Application.persistentDataPath + "/metaStats.json", json);
        json = JsonUtility.ToJson(townStats);
        File.WriteAllText(Application.persistentDataPath + "/townStats.json", json);
    }
    
    public void Load()
    {
        metaStats = new RunStats();
        if (!File.Exists(Application.persistentDataPath + "/metaStats.json"))
        {
            File.Create(Application.persistentDataPath + "/metaStats.json").Dispose();
        } 
        else
        {
            string metaStatsJson = File.ReadAllText(Application.persistentDataPath + "/metaStats.json");
            metaStats = JsonUtility.FromJson<RunStats>(metaStatsJson);
        }

        if (!File.Exists(Application.persistentDataPath + "/townStats.json"))
        {
            File.Create(Application.persistentDataPath + "/townStats.json").Dispose();
        }
        else
        {
            string townStatsJson = File.ReadAllText(Application.persistentDataPath + "/townStats.json");
            townStats = JsonUtility.FromJson<TownStats>(townStatsJson);
        }
    }
}
