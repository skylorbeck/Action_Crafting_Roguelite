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

    public ObjectPool<ExperienceOrb> experienceOrbs;
    public List<ExperienceOrb> activeExperienceOrbs = new List<ExperienceOrb>();
    public ExperienceOrb experienceOrbPrefab;

    public ObjectPool<GoldCoin> coins;
    public List<GoldCoin> activeCoins = new List<GoldCoin>();
    public GoldCoin coinPrefab;
    
    private async void Awake()
    {
        instance = this;
        resourceNodes = new ObjectPool<ResourceNode>(
            () =>
            {
                ResourceNode resourceNode = Instantiate(resourceDropRegistry.GetResourceNodePrefab(Random.value > 0.5f ?ResourceDrop.Resource.Wood : ResourceDrop.Resource.Stone),transform); 
                return resourceNode;
            },
            resourceNode =>
            {
                resourceNode.gameObject.SetActive(true);
                resourceNode.transform.DOKill();
                resourceNode.transform.localScale = Vector3.one;
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
                ResourceDrop resourceDrop = Instantiate(resourceDropRegistry.GetResourceDropPrefab(ResourceDrop.Resource.Stone),transform); 
                return resourceDrop;
            },
            resourceDrop =>
            {
                resourceDrop.collider.enabled = false;
                resourceDrop.gameObject.SetActive(true);
                resourceDrop.transform.DOKill();
                resourceDrop.transform.localScale = Vector3.one;
                activeDrops.Add(resourceDrop);
            },
            resourceDrop =>
            {
                resourceDrop.collider.enabled = false;
                activeDrops.Remove(resourceDrop);
                resourceDrop.gameObject.SetActive(false);
            }
        );
        experienceOrbs = new ObjectPool<ExperienceOrb>(
            () =>
            {
                ExperienceOrb experienceOrb = Instantiate(experienceOrbPrefab,transform); 
                return experienceOrb;
            },
            experienceOrb =>
            {
                experienceOrb.collider.enabled = false;
                experienceOrb.gameObject.SetActive(true);
                experienceOrb.transform.DOKill();
                experienceOrb.transform.localScale = Vector3.one;
                activeExperienceOrbs.Add(experienceOrb);
            },
            experienceOrb =>
            {
                experienceOrb.collider.enabled = false;
                activeExperienceOrbs.Remove(experienceOrb);
                experienceOrb.gameObject.SetActive(false);
            }
        );
        coins = new ObjectPool<GoldCoin>(
            () =>
            {
                GoldCoin coin = Instantiate(coinPrefab,transform); 
                return coin;
            },
            coin =>
            {
                coin.collider.enabled = false;
                coin.gameObject.SetActive(true);
                coin.transform.DOKill();
                coin.transform.localScale = Vector3.one;
                activeCoins.Add(coin);
            },
            coin =>
            {
                coin.collider.enabled = false;
                activeCoins.Remove(coin);
                coin.gameObject.SetActive(false);
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
        ResourceDrop resourceDrop = resourceDrops.Get();
        resourceDrop.SetResource(resource);
        resourceDrop.SetAmount(1);//TODO perk influence
        resourceDrop.transform.position = position;
        resourceDrop.transform.DOJump(position + (Random.insideUnitCircle*resourceNodeSpawnRadius), 0.5f, 2, 0.5f).onComplete += () =>
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
    
    public void SpawnExperienceOrb(Vector2 position, uint amount)
    {
        ExperienceOrb experienceOrb = experienceOrbs.Get();
        experienceOrb.SetAmount(amount);
        experienceOrb.transform.position = position;
        experienceOrb.transform.DOJump(position + (Random.insideUnitCircle*resourceNodeSpawnRadius), 0.5f, 2, 0.5f).onComplete += () =>
        {
            experienceOrb.collider.enabled = true;
            experienceOrb.transform.DOScale(1.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        };
    }
    
    public void ReleaseExperienceOrb(ExperienceOrb experienceOrb)
    {
        experienceOrbs.Release(experienceOrb);
    }
    
    public void ReleaseAllExperienceOrbs()
    {
        List<ExperienceOrb> activeExperienceOrbs = new List<ExperienceOrb>(this.activeExperienceOrbs);
        foreach (var experienceOrb in activeExperienceOrbs)
        {
            experienceOrbs.Release(experienceOrb);
        }
    }

    public void CleanupExperienceOrbs()
    {
        //TODO merge old experience orbs with new experience orbs for more value
    }
    
    public void SpawnCoin(Vector2 position, uint amount)
    {
        GoldCoin coin = coins.Get();
        coin.SetAmount(amount);
        coin.transform.position = position;
        coin.transform.DOJump(position + (Random.insideUnitCircle*resourceNodeSpawnRadius), 0.5f, 1, 0.25f).onComplete += () =>
        {
            coin.collider.enabled = true;
            coin.transform.DOScale(1.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        };
    }
    
    public void ReleaseCoin(GoldCoin coin)
    {
        coins.Release(coin);
    }
    
    public void ReleaseAllCoins()
    {
        List<GoldCoin> activeCoins = new List<GoldCoin>(this.activeCoins);
        foreach (var coin in activeCoins)
        {
            coins.Release(coin);
        }
    }

    public void CleanupCoins()
    {
        //TODO merge old coins with new coins for more value
    }

    public void ReleaseAllAll()
    {
        ReleaseAllCoins();
        ReleaseAllExperienceOrbs();
        ReleaseAllResourceDrops();
        ReleaseAllResourceNodes();
    }
}
