using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceDropRegistry", menuName = "ResourceDropRegistry")]
public class ResourceDropRegistry : ScriptableObject, ISerializationCallbackReceiver
{
    public List<ResourceDrop.Resource> resourceList = new List<ResourceDrop.Resource>();
    public List<string> resourceNameList = new List<string>();
    public List<string> resourceDescriptionList = new List<string>();
    public List<ResourceNode> resourceNodePrefabList = new List<ResourceNode>();
    public List<ResourceDrop> resourceDropPrefabList = new List<ResourceDrop>();
    public List<uint> resourceValueList = new List<uint>();

    public Dictionary<ResourceDrop.Resource, string> resourceNames = new Dictionary<ResourceDrop.Resource, string>();
    public Dictionary<ResourceDrop.Resource, string> resourceDescriptions = new Dictionary<ResourceDrop.Resource, string>();
    public Dictionary<ResourceDrop.Resource, ResourceNode> resourceNodePrefabs = new Dictionary<ResourceDrop.Resource, ResourceNode>();
    public Dictionary<ResourceDrop.Resource, ResourceDrop> resourceDropPrefabs = new Dictionary<ResourceDrop.Resource, ResourceDrop>();
    public Dictionary<ResourceDrop.Resource, uint> resourceValues = new Dictionary<ResourceDrop.Resource, uint>();

    public ResourceNode GetResourceNodePrefab(ResourceDrop.Resource resource)
    {
        return resourceNodePrefabs.ContainsKey(resource) ? resourceNodePrefabs[resource] : null;
    }

    public ResourceDrop GetResourceDropPrefab(ResourceDrop.Resource resource)
    {
        return resourceDropPrefabs.ContainsKey(resource) ? resourceDropPrefabs[resource] : null;
    }
    
    public string GetResourceName(ResourceDrop.Resource resource)
    {
        return resourceNames.ContainsKey(resource) ? resourceNames[resource] : "";
    }
    
    public string GetResourceDescription(ResourceDrop.Resource resource)
    {
        return resourceDescriptions.ContainsKey(resource) ? resourceDescriptions[resource] : "";
    }
    
    public uint GetResourceValue(ResourceDrop.Resource resource)
    {
        return resourceValues.ContainsKey(resource) ? resourceValues[resource] : (uint)0;
    }
    
    
    public void OnBeforeSerialize()
    {
        resourceNames.Clear();
        resourceDescriptions.Clear();
        resourceNodePrefabs.Clear();
        resourceDropPrefabs.Clear();
        resourceValues.Clear();
    }

    public void OnAfterDeserialize()
    {
        //make all Lists the same size
        while (resourceList.Count > resourceNameList.Count)
        {
            resourceNameList.Add("");
        }
        while (resourceList.Count > resourceDescriptionList.Count)
        {
            resourceDescriptionList.Add("");
        }
        while (resourceList.Count > resourceNodePrefabList.Count)
        {
            resourceNodePrefabList.Add(null);
        }
        while (resourceList.Count > resourceDropPrefabList.Count)
        {
            resourceDropPrefabList.Add(null);
        }
        while (resourceList.Count > resourceValueList.Count)
        {
            resourceValueList.Add(0);
        }

        while (resourceList.Count < resourceNameList.Count)
        {
            resourceNameList.RemoveAt(resourceNameList.Count - 1);
        }
        while (resourceList.Count < resourceDescriptionList.Count)
        {
            resourceDescriptionList.RemoveAt(resourceDescriptionList.Count - 1);
        }
        while (resourceList.Count < resourceNodePrefabList.Count)
        {
            resourceNodePrefabList.RemoveAt(resourceNodePrefabList.Count - 1);
        }
        while (resourceList.Count < resourceDropPrefabList.Count)
        {
            resourceDropPrefabList.RemoveAt(resourceDropPrefabList.Count - 1);
        }
        while (resourceList.Count < resourceValueList.Count)
        {
            resourceValueList.RemoveAt(resourceValueList.Count - 1);
        }
        
        

        for (int i = 0; i < resourceList.Count; i++)
        {
            resourceNames.Add(resourceList[i], resourceNameList[i]);
            resourceDescriptions.Add(resourceList[i], resourceDescriptionList[i]);
            resourceNodePrefabs.Add(resourceList[i], resourceNodePrefabList[i]);
            resourceDropPrefabs.Add(resourceList[i], resourceDropPrefabList[i]);
            resourceValues.Add(resourceList[i], resourceValueList[i]);
        }
    }
}