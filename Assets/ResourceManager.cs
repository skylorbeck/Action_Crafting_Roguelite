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
    
    public bool spawnResourceNodes = true;
    
    public ResourceDropRegistry resourceDropRegistry;
    public ResourceNodeRegistry resourceNodeRegistry;

    public ObjectPool<ResourceNode> resourceNodes;
    public List<ResourceNode> activeNodes = new List<ResourceNode>();
    public uint resourceNodeCap = 10;
    public Transform resourceNodeParent;

    public ObjectPool<ResourceDrop> resourceDrops;
    public List<ResourceDrop> activeDrops = new List<ResourceDrop>();
    public uint resourceDropCap = 10;
    public float resourceNodeSpawnRadius = 10f;
    public Transform resourceDropParent;

    public ObjectPool<ExperienceOrb> experienceOrbs;
    public List<ExperienceOrb> activeExperienceOrbs = new List<ExperienceOrb>();
    public uint expOrbCap = 10;
    public ExperienceOrb experienceOrbPrefab;
    public Transform experienceOrbParent;

    public ObjectPool<GoldCoin> coins;
    public List<GoldCoin> activeCoins = new List<GoldCoin>();
    public uint coinCap = 10;
    public GoldCoin coinPrefab;
    public Transform coinParent;

    private async void Awake()
    {
        instance = this;
        resourceNodes = new ObjectPool<ResourceNode>(
            () =>
            {
                ResourceNode resourceNode =
                    Instantiate(
                        resourceDropRegistry.GetResourceNodePrefab(Random.value > 0.5f
                            ? ResourceDrop.Resource.Wood
                            : ResourceDrop.Resource.Stone), resourceNodeParent);
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
                ResourceDrop resourceDrop =
                    Instantiate(resourceDropRegistry.GetResourceDropPrefab(ResourceDrop.Resource.Stone),
                        resourceDropParent);
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
                ExperienceOrb experienceOrb = Instantiate(experienceOrbPrefab, experienceOrbParent);
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
                GoldCoin coin = Instantiate(coinPrefab, coinParent);
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

    #region resourceNodes

    private void CheckForRoomAndSpawnResourceNode()
    {
        if (activeNodes.Count < resourceNodeCap && spawnResourceNodes)
        {
            SpawnResourceNode(Random.value > 0.5f ? ResourceNode.Resource.Wood : ResourceNode.Resource.Stone);
            //TODO at some point in the future we will need a way to influence which nodes spawn and when, instead of randomly spawning them. 
        }
    }


    public void SpawnResourceNode(ResourceNode.Resource resource = ResourceNode.Resource.Stone)
    {
        SpawnResourceNode(
            Player.instance.transform.position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10)), resource);
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

    #endregion

    #region resourceDrops

    
    public void SpawnResourceDrop(Vector2 position, ResourceDrop.Resource resource = ResourceDrop.Resource.Stone)
    {
        ResourceDrop resourceDrop = resourceDrops.Get();
        resourceDrop.SetResource(resource);
        resourceDrop.SetAmount(1); //TODO perk influence
        resourceDrop.transform.position = position;
        resourceDrop.transform.DOJump(position + (Random.insideUnitCircle * resourceNodeSpawnRadius), 0.5f, 2, 0.5f)
            .onComplete += () =>
        {
            resourceDrop.collider.enabled = true;
            resourceDrop.transform.DOScale(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        };
        if (resourceDrops.CountActive > resourceDropCap)
        {
            CleanupDrops();
        }
    }

    public void SpawnResourceDrop(Vector2 position, uint value, ResourceDrop.Resource resource)
    {
        ResourceDrop resourceDrop = resourceDrops.Get();
        resourceDrop.SetResource(resource);
        resourceDrop.SetAmount(value);
        resourceDrop.transform.position = position;
        resourceDrop.collider.enabled = true;
        resourceDrop.transform.DOScale(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
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
        List<ResourceDrop> allDrops = new List<ResourceDrop>(this.activeDrops);
        Vector3 playerPosition = Player.instance.transform.position;
        
        allDrops.Sort((a, b) => Vector3.Distance(a.transform.position, playerPosition)
            .CompareTo(Vector3.Distance(b.transform.position, playerPosition)));
        allDrops.Reverse();
        
        for (int i = 0; i < allDrops.Count - resourceDropCap; i++)
        {
            ResourceDrop resourceDrop = allDrops[i];
            ResourceDrop lastDrop = allDrops.FindLast(d => d.GetResource() == resourceDrop.GetResource());
            if (lastDrop != null && lastDrop != resourceDrop)
            {
                lastDrop.SetAmount(lastDrop.GetAmount() + resourceDrop.GetAmount());
                resourceDrops.Release(resourceDrop);
                
            }
            else
            {
                Debug.LogError("Could not find a resource drop to merge with");
            }
        }
    }

    #endregion

    #region expOrbs

    public void SpawnExperienceOrb(Vector2 position, uint amount)
    {
        for (int i = 0; i < amount; i++)
        {
            ExperienceOrb experienceOrb = experienceOrbs.Get();
            experienceOrb.SetAmount(1);
            experienceOrb.transform.position = position;
            experienceOrb.transform
                    .DOJump(position + (Random.insideUnitCircle * resourceNodeSpawnRadius), 0.5f, 2, 0.5f).onComplete +=
                () =>
                {
                    experienceOrb.collider.enabled = true;
                    experienceOrb.transform.DOScale(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
                };
        }

        if (activeExperienceOrbs.Count > expOrbCap)
        {
            CleanupExperienceOrbs();
        }
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
        List<ExperienceOrb> allExperienceOrbs = new List<ExperienceOrb>(this.activeExperienceOrbs);
        Vector3 playerPosition = Player.instance.transform.position;
        
        allExperienceOrbs.Sort((a, b) => Vector3.Distance(a.transform.position, playerPosition)
            .CompareTo(Vector3.Distance(b.transform.position, playerPosition)));
        allExperienceOrbs.Reverse();
        
        for (int i = 0; i < allExperienceOrbs.Count - expOrbCap; i++)
        {
            ExperienceOrb experienceOrb = allExperienceOrbs[i];
            ExperienceOrb lastExperienceOrb = allExperienceOrbs[^1];
            if (lastExperienceOrb != null && lastExperienceOrb != experienceOrb)
            {
                lastExperienceOrb.SetAmount(lastExperienceOrb.GetAmount() + experienceOrb.GetAmount());
                experienceOrbs.Release(experienceOrb);
                
            }
            else
            {
                Debug.LogError("Could not find a experience orb to merge with");
            }
        }
    }

    #endregion

    #region coins

    public void SpawnCoin(Vector2 position, uint amount)
    {
        GoldCoin coin = coins.Get();
        coin.SetAmount(amount);
        coin.transform.position = position;
        coin.transform.DOJump(position + (Random.insideUnitCircle * resourceNodeSpawnRadius), 0.5f, 1, 0.25f)
            .onComplete += () =>
        {
            coin.collider.enabled = true;
            coin.transform.DOScale(1.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        };
        if (coins.CountActive > coinCap)
        {
            CleanupCoins();
        }
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
        List<GoldCoin> allCoins = new List<GoldCoin>(this.activeCoins);
        Vector3 playerPosition = Player.instance.transform.position;
        
        allCoins.Sort((a, b) => Vector3.Distance(a.transform.position, playerPosition)
            .CompareTo(Vector3.Distance(b.transform.position, playerPosition)));
        allCoins.Reverse();
        
        for (int i = 0; i < allCoins.Count - coinCap; i++)
        {
            GoldCoin coin = allCoins[i];
            GoldCoin lastCoin = allCoins[^1];
            if (lastCoin != null && lastCoin != coin)
            {
                lastCoin.SetAmount(lastCoin.GetAmount() + coin.GetAmount());
                coins.Release(coin);
                
            }
            else
            {
                Debug.LogError("Could not find a coin to merge with");
            }
        }
    }

    #endregion

    public void ReleaseAllAll()
    {
        ReleaseAllCoins();
        ReleaseAllExperienceOrbs();
        ReleaseAllResourceDrops();
        ReleaseAllResourceNodes();
    }

    public void SetSpawnResources(bool b)
    {
        spawnResourceNodes = b;
    }
}