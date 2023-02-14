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
    public uint goldCollected;
    //version 1 end
    
    public RunStats()
    {
        Reset();
    }
    public void AddRunStats(RunStats runStats)
    {
        enemiesKilled += runStats.enemiesKilled;
        resourcesCollected += runStats.resourcesCollected;
        stoneCollected += runStats.stoneCollected;
        stoneNodesHarvested += runStats.stoneNodesHarvested;
        woodCollected += runStats.woodCollected;
        woodNodesHarvested += runStats.woodNodesHarvested;
        goldCollected += runStats.goldCollected;
    }
    
    public void Reset()
    {
        enemiesKilled = 0;
        resourcesCollected = 0;
        stoneCollected = 0;
        stoneNodesHarvested = 0;
        woodCollected = 0;
        woodNodesHarvested = 0;
        goldCollected = 0;
    }
    
  
}