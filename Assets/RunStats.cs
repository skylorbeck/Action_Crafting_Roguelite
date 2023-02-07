using System;
using UnityEngine;

[Serializable]
public class RunStats
{
    public uint version = 1;
    public uint enemiesKilled;
    public uint resourcesCollected;
    public uint stoneCollected;
    public uint stoneNodesHarvested;
    public uint woodCollected;
    public uint woodNodesHarvested;
    //version 1 end
    public void AddRunStats(RunStats runStats)
    {
        enemiesKilled += runStats.enemiesKilled;
        resourcesCollected += runStats.resourcesCollected;
        stoneCollected += runStats.stoneCollected;
        stoneNodesHarvested += runStats.stoneNodesHarvested;
        woodCollected += runStats.woodCollected;
        woodNodesHarvested += runStats.woodNodesHarvested;
    }
    
    public void Reset()
    {
        enemiesKilled = 0;
        resourcesCollected = 0;
        stoneCollected = 0;
        stoneNodesHarvested = 0;
        woodCollected = 0;
        woodNodesHarvested = 0;
    }
    
    public bool InsertSaveData(RunStats saveFile)
    {
        if (saveFile.version < 1)
        {
            return false;
        }
        enemiesKilled = saveFile.enemiesKilled;
        resourcesCollected = saveFile.resourcesCollected;
        stoneCollected = saveFile.stoneCollected;
        stoneNodesHarvested = saveFile.stoneNodesHarvested;
        woodCollected = saveFile.woodCollected;
        woodNodesHarvested = saveFile.woodNodesHarvested;
        return true;
    }
}