using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
    public ResourceDropRegistry resourceDropRegistry;
    public ResourceNodeRegistry resourceNodeRegistry;
    
    public ObjectPool<ResourceNode> resourceNodes;
    public List<ResourceNode> activeNodes = new List<ResourceNode>();

    public ObjectPool<ResourceDrop> resourceDrops;
    public List<ResourceDrop> activeDrops = new List<ResourceDrop>();
    
    public float resourceNodeSpawnRadius = 10f;

    private async void Awake()
    {
        instance = this;
        resourceNodes = new ObjectPool<ResourceNode>(
            () =>
            {
                ResourceNode resourceNode = Instantiate(resourceDropRegistry.GetResourceNodePrefab(Random.value > 0.5f ?ResourceDrop.Resource.Wood : ResourceDrop.Resource.Stone)); 
                return resourceNode;
            },
            resourceNode =>
            {
                resourceNode.gameObject.SetActive(true);
                activeNodes.Add(resourceNode);
            },
            resourceNode =>
            {
                activeNodes.Remove(resourceNode);
                resourceNode.gameObject.SetActive(false);
            }
        );
        resourceDrops = new ObjectPool<ResourceDrop>(
            () =>
            {
                ResourceDrop resourceDrop = Instantiate(resourceDropRegistry.GetResourceDropPrefab(ResourceDrop.Resource.Stone)); 
                return resourceDrop;
            },
            resourceDrop =>
            {
                resourceDrop.gameObject.SetActive(true);
                activeDrops.Add(resourceDrop);
            },
            resourceDrop =>
            {
                activeDrops.Remove(resourceDrop);
                resourceDrop.gameObject.SetActive(false);
            }
        );
        await Task.Delay(1);
        TimerManager.instance.onOneSecond += CheckForRoomAndSpawnResourceNode;
    }

    private void CheckForRoomAndSpawnResourceNode()
    {
        if (activeNodes.Count < 10)
        {
            SpawnResourceNode(Random.value > 0.5f ? ResourceNode.Resource.Wood : ResourceNode.Resource.Stone);
            //TODO at some point in the future we will need a way to influence which nodes spawn and when, instead of randomly spawning them. 
        }
    }

    
    public void SpawnResourceNode(ResourceNode.Resource resource = ResourceNode.Resource.Stone)
    {
        SpawnResourceNode(Player.instance.transform.position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10)),resource);
    }

    public void SpawnResourceNode(Vector3 position, ResourceNode.Resource resource = ResourceNode.Resource.Stone)
    {
        var resourceNode = resourceNodes.Get();
        ResourceNode newResource = resourceNodeRegistry.GetResourceNodePrefab(resource);
        resourceNode.SetResource(newResource);
        resourceNode.transform.position = position;
        resourceNode.transform.DOJump(position, 1f, 1, 0.25f); 
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

    public void SpawnResourceDrop(Vector2 position, ResourceDrop.Resource resource = ResourceDrop.Resource.Stone)
    {
        ResourceDrop resourceDrop = Instantiate(resourceDropRegistry.GetResourceDropPrefab(resource));
        resourceDrop.SetResource(resource);
        resourceDrop.SetAmount(1);//TODO perk influence
        resourceDrop.transform.position = position;
        resourceDrop.transform.DOJump(position + (Random.insideUnitCircle*resourceNodeSpawnRadius), 0.5f, 1, 0.25f).onComplete += () =>
        {
            resourceDrop.collider.enabled = true;
            resourceDrop.transform.DOScale(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        };
        
    }
    
    public void ReleaseResourceDrop(ResourceDrop resourceDrop)
    {
        resourceDrops.Release(resourceDrop);
    }
    
    public void ReleaseAllResourceDrops()
    {
        List<ResourceDrop> activeDrops = new List<ResourceDrop>(this.activeDrops);
        foreach (var resourceDrop in activeDrops)
        {
            resourceDrops.Release(resourceDrop);
        }
    }

    public void CleanupDrops()
    {
        //TODO merge old drops with new drops for more value
    }
}
