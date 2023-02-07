using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    [SerializeField] private RunStats metaStats; 
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
    
    public void Save()
    {
        string json = JsonUtility.ToJson(metaStats);
        File.WriteAllText(Application.persistentDataPath + "/metaStats.json", json);
    }
    
    public bool Load() //returns false if no save file or save file is corrupt
    {
        metaStats = new RunStats();
        if (!File.Exists(Application.persistentDataPath + "/metaStats.json"))
        {
            File.Create(Application.persistentDataPath + "/metaStats.json").Dispose();
            return false;
        }
        string json = File.ReadAllText(Application.persistentDataPath + "/metaStats.json");
        bool success = metaStats.InsertSaveData(JsonUtility.FromJson<RunStats>(json));
        return success;
    }
}
