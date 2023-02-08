using System;
using UnityEngine;

[Serializable]
public class TownStats
{
    public uint version = 1;

    public uint experience = 0;
    public uint experienceToNextLevel = 100;
    public uint level = 0;

    

    //version 1 end
    public void AddTownStats(TownStats townStats)
    {
        experience += townStats.experience;
        experienceToNextLevel = townStats.experienceToNextLevel;
        level = townStats.level;
    }
    
    public void SetExperience(uint experienceValue)
    {
        experience = experienceValue;
    }
    
    public void SetLevel(uint levelValue)
    {
        level = levelValue;
    }
    
    public void SetExperienceToNextLevel(uint experienceToNextLevelValue)
    {
        experienceToNextLevel = experienceToNextLevelValue;
    }
    
    public void Reset()
    {
        experience = 0;
        experienceToNextLevel = 20;
        level = 0;
    }
    
    public bool InsertSaveData(TownStats saveFile)
    {
        if (saveFile.version < 1)
        {
            return false;
        }
        experience = saveFile.experience;
        return true;
    }
}