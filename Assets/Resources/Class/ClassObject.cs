using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Class", menuName = "ClassObject")]
public class ClassObject : ScriptableObject
{
    public ResourceNode.Resource targetResource;
    public ResourceDrop.Resource[] unlockResources;
    public int[] unlockAmount;
    public int unlockLevel;
    public int unlockCost;
    
    public Perk[] perkList;
    
    public string className;
    public string classDescription;
    public Sprite[] classSprites;
    
    
}
