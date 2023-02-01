using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceNodeRegistry", menuName = "ResourceNodeRegistry")]
public class ResourceNodeRegistry : ScriptableObject, ISerializationCallbackReceiver
{
    public List<ResourceNode.Resource> resourceList = new List<ResourceNode.Resource>();
    public List<string> resourceNameList = new List<string>();
    public List<string> resourceDescriptionList = new List<string>();
    public List<ResourceNode> resourceNodePrefabList = new List<ResourceNode>();

    public Dictionary<ResourceNode.Resource, string> resourceNames = new Dictionary<ResourceNode.Resource, string>();
    public Dictionary<ResourceNode.Resource, string> resourceDescriptions = new Dictionary<ResourceNode.Resource, string>();
    public Dictionary<ResourceNode.Resource, ResourceNode> resourceNodePrefabs = new Dictionary<ResourceNode.Resource, ResourceNode>();

    public ResourceNode GetResourceNodePrefab(ResourceNode.Resource resource)
    {
        return resourceNodePrefabs.ContainsKey(resource) ? resourceNodePrefabs[resource] : null;
    }

    public string GetResourceName(ResourceNode.Resource resource)
    {
        return resourceNames.ContainsKey(resource) ? resourceNames[resource] : "";
    }
    
    public string GetResourceDescription(ResourceNode.Resource resource)
    {
        return resourceDescriptions.ContainsKey(resource) ? resourceDescriptions[resource] : "";
    }
    
    
    
    public void OnBeforeSerialize()
    {
        resourceNames.Clear();
        resourceDescriptions.Clear();
        resourceNodePrefabs.Clear();
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
        
        

        for (int i = 0; i < resourceList.Count; i++)
        {
            resourceNames.Add(resourceList[i], resourceNameList[i]);
            resourceDescriptions.Add(resourceList[i], resourceDescriptionList[i]);
            resourceNodePrefabs.Add(resourceList[i], resourceNodePrefabList[i]);
        }
    }
}