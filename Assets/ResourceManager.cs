using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
    public ObjectPool<ResourceNode> resourceNodes;
    public List<ResourceNode> activeNodes = new List<ResourceNode>();
    public ResourceNode stonePrefab;
    public ResourceNode woodPrefab;
//TODO at some point in the future we will need a way to influence which nodes spawn and when, instead of randomly spawning them. 

    private async void Awake()
    {
        instance = this;
        resourceNodes = new ObjectPool<ResourceNode>(
            () =>
            {
                ResourceNode resourceNode = Random.value > 0.5f ? Instantiate(woodPrefab) : Instantiate(stonePrefab);
                return resourceNode;
            },
            resourceNode =>
            {
                ResourceNode newResource = Random.value > 0.5f ? woodPrefab :stonePrefab;
                resourceNode.gameObject.SetActive(true);
                resourceNode.SetResource(newResource);
                activeNodes.Add(resourceNode);
            },
            resourceNode =>
            {
                activeNodes.Remove(resourceNode);
                resourceNode.gameObject.SetActive(false);
            }
        );
        await Task.Delay(1);
        TimerManager.instance.onOneSecond += CheckForRoomAndSpawnResourceNode;
    }

    private void CheckForRoomAndSpawnResourceNode()
    {
        if (activeNodes.Count < 10)
        {
            SpawnResourceNode();
        }
    }

    private void SpawnResourceNode()
    {
        SpawnResourceNode(Player.instance.transform.position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10)));
    }

    public void SpawnResourceNode(Vector3 position)
    {
        var resourceNode = resourceNodes.Get();
        resourceNode.transform.position = position;
    }
    
    public void ReleaseResourceNode(ResourceNode resourceNode)
    {
        resourceNodes.Release(resourceNode);
    }
    
    public void ReleaseAllResourceNodes()
    {
        List<ResourceNode> activeNodes = new List<ResourceNode>(this.activeNodes);
        foreach (var resourceNode in activeNodes)
        {
            resourceNodes.Release(resourceNode);
        }
    }
    
}
