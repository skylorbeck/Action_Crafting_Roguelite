using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [SerializeField] private RunStats metaStats; 
    [SerializeField] private TownStats townStats; 
    [SerializeField] private MetaUpgrades metaUpgrades;
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

    public MetaUpgrades GetMetaUpgrades()
    {
        return metaUpgrades;
    }
    
    public void Save()
    {
        Save("metaStats", metaStats);
        Save("townStats", townStats);
        Save("metaUpgrades", metaUpgrades);
    }
    
    public void Load()
    {
        Load("metaStats", out metaStats);
        Load("townStats", out townStats);
        Load("metaUpgrades", out metaUpgrades);
    }
    
    public void Load<T>(string path, out T data)
    {
        path = Application.persistentDataPath + "/" + path + ".json";
        if (!File.Exists(path))
        {
            File.Create(path).Dispose();
            data = default;
        }
        else
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<T>(json);
        }
    }
    
    public void Save<T>(string path, T data)
    {
        path = Application.persistentDataPath + "/" + path + ".json";
        if (!File.Exists(path))
        {
            File.Create(path).Dispose();
        }
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }
}
