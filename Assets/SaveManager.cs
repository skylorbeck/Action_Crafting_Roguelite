using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public bool loaded = false;
    [SerializeField] private RunStats metaStats; 
    [SerializeField] private TownStats townStats; 
    [SerializeField] private MetaUpgrades metaUpgrades;
    [SerializeField] private PlayerToolData playerToolData;
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
    
    public PlayerToolData GetPlayerToolData()
    {
        return playerToolData;
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
        Save("playerToolData", playerToolData);
    }
    
    public void Load()
    {
        Load("metaStats", out metaStats);
        Load("townStats", out townStats);
        Load("metaUpgrades", out metaUpgrades);
        Load("playerToolData", out playerToolData);
        loaded = true;
    }
    
    public void Load<T>(string path, out T data) where T : new()
    {
        path = Application.persistentDataPath + "/" + path + ".json";
        if (!File.Exists(path))
        {
            File.Create(path).Dispose();
            data = new T();
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(path, json);
        }
        else
        {
            string json = File.ReadAllText(path);
            if (json == "")
            {
                Debug.Log("File" + path + " is empty");
                data = new T();
                json = JsonUtility.ToJson(data);
                File.WriteAllText(path, json);
            }
            else
            {
                data = JsonUtility.FromJson<T>(json);
            }
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
