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

    public float pulseSize = 0.9f;
    public float pulseSpeed = 0.5f;
    
    [SerializeField] private Vector2Int spawnRange = new Vector2Int(30, 30);//TODO Move this to a global settings class

    public ObjectPool<ResourceNode> resourceNodes;
    public List<ResourceNode> activeNodes = new List<ResourceNode>();
    public uint resourceNodeCap = 10;
    public uint resourceNodeSpawnCount = 1;
    public Transform resourceNodeParent;
    
    public ObjectPool<ResourceDrop> resourceDrops;
    public List<ResourceDrop> activeDrops = new List<ResourceDrop>();
    public uint resourceDropCap = 10;
    public float resourceNodeSpawnRadius = 10f;
    public Transform resourceDropParent;
    
    public ObjectPool<ExperienceOrb> experienceOrbs;
    public List<ExperienceOrb> activeExperienceOrbs = new List<ExperienceOrb>();
    public uint expOrbCap = 100;
    public uint expPerOrb = 5;
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
                resourceDrop.transform.localScale = Vector3.one *0.5f;
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
                experienceOrb.transform.localScale = Vector3.one *0.5f;
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
                coin.transform.localScale = Vector3.one *0.5f;
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
            for (int i = 0; i < resourceNodeSpawnCount; i++)
            {
                SpawnResourceNode(Random.value > 0.5f ? ResourceNode.Resource.Wood : ResourceNode.Resource.Stone);
                //TODO at some point in the future we will need a way to influence which nodes spawn and when, instead of randomly spawning them. 
            }
        }
    }


    public void SpawnResourceNode(ResourceNode.Resource resource = ResourceNode.Resource.Stone)
    {
        Vector3 pos;
        do
        {
            pos = new Vector3(Random.Range(-spawnRange.x, spawnRange.x), Random.Range(-spawnRange.y, spawnRange.y), 0);
        } while (pos.x<-spawnRange.x+1.5f || pos.x>spawnRange.x-1.5f || pos.y<-spawnRange.y+1.5f || pos.y>spawnRange.y-1.5f);
        SpawnResourceNode(pos, resource);
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
        if (!resourceNode.gameObject.activeSelf)return;
        resourceNodes.Release(resourceNode);
    }

    public void ReleaseAllResourceNodes()
    {
        List<ResourceNode> activeNodes = new List<ResourceNode>(this.activeNodes);
        foreach (var resourceNode in activeNodes)
        {
            ReleaseResourceNode(resourceNode);
        }
    }

    #endregion

    #region resourceDrops

    
    public void SpawnResourceDrop(Vector2 position, ResourceDrop.Resource resource = ResourceDrop.Resource.Stone)
    {
        ResourceDrop resourceDrop = resourceDrops.Get();
        resourceDrop.SetResource(resource);
        resourceDrop.SetAmount(1);
        resourceDrop.transform.position = position;
        Vector3 targetPosition = position + (Random.insideUnitCircle * resourceNodeSpawnRadius);
        targetPosition = new Vector2(Mathf.Clamp(targetPosition.x, -spawnRange.x+1.5f, spawnRange.x-1.5f), Mathf.Clamp(targetPosition.y, -spawnRange.y+1.5f, spawnRange.y-1.5f));
        resourceDrop.transform.DOJump(targetPosition, 0.5f, 2, 0.5f)
            .onComplete += () =>
        {
            resourceDrop.collider.enabled = true;
            resourceDrop.transform.DOScale(pulseSize, pulseSpeed).SetLoops(-1, LoopType.Yoyo);
        };
        if (resourceDrops.CountActive > resourceDropCap)
        {
            CleanupDrops();
        }
    }
    
    public void ReleaseResourceDrop(ResourceDrop resourceDrop)
    {
        if (!resourceDrop.gameObject.activeSelf) return;
        resourceDrops.Release(resourceDrop);
    }

    public void ReleaseAllResourceDrops()
    {
        List<ResourceDrop> activeDrops = new List<ResourceDrop>(this.activeDrops);
        foreach (var resourceDrop in activeDrops)
        {
            ReleaseResourceDrop(resourceDrop);
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
        amount = (uint)Mathf.RoundToInt(amount * Player.instance.GetExperienceBonus());
        uint remainingAmount = (uint)Mathf.RoundToInt(amount%expPerOrb);
        int orbAmount = Mathf.RoundToInt(amount/(float)expPerOrb);
        for (int i = 0; i < orbAmount; i++)
        {
            ExperienceOrb experienceOrb = experienceOrbs.Get();
            experienceOrb.SetAmount(expPerOrb);
            experienceOrb.transform.position = position;
            Vector3 targetPosition = position + (Random.insideUnitCircle * resourceNodeSpawnRadius);
            targetPosition = new Vector2(Mathf.Clamp(targetPosition.x, -spawnRange.x+1.5f, spawnRange.x-1.5f), Mathf.Clamp(targetPosition.y, -spawnRange.y+1.5f, spawnRange.y-1.5f));
            experienceOrb.transform
                    .DOJump(targetPosition, 0.5f, 2, 0.5f).onComplete +=
                () =>
                {
                    experienceOrb.collider.enabled = true;
                    experienceOrb.transform.DOScale(pulseSize, pulseSpeed).SetLoops(-1, LoopType.Yoyo);
                };
        }
        
        if (remainingAmount > 0)
        {
            ExperienceOrb experienceOrb = experienceOrbs.Get();
            experienceOrb.SetAmount(remainingAmount);
            experienceOrb.transform.position = position;
            Vector3 targetPosition = position + (Random.insideUnitCircle * resourceNodeSpawnRadius);
            targetPosition = new Vector2(Mathf.Clamp(targetPosition.x, -spawnRange.x+1.5f, spawnRange.x-1.5f), Mathf.Clamp(targetPosition.y, -spawnRange.y+1.5f, spawnRange.y-1.5f));
            experienceOrb.transform
                    .DOJump(targetPosition, 0.5f, 2, 0.5f).onComplete +=
                () =>
                {
                    experienceOrb.collider.enabled = true;
                    experienceOrb.transform.DOScale(pulseSize, pulseSpeed).SetLoops(-1, LoopType.Yoyo);
                };
        }

        if (activeExperienceOrbs.Count > expOrbCap)
        {
            CleanupExperienceOrbs();
        }
    }

    public void ReleaseExperienceOrb(ExperienceOrb experienceOrb)
    {
        if (!experienceOrb.gameObject.activeSelf) return;
        experienceOrbs.Release(experienceOrb);
    }

    public void ReleaseAllExperienceOrbs()
    {
        List<ExperienceOrb> activeExperienceOrbs = new List<ExperienceOrb>(this.activeExperienceOrbs);
        foreach (var experienceOrb in activeExperienceOrbs)
        {
            ReleaseExperienceOrb(experienceOrb);
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
            ExperienceOrb lastExperienceOrb = allExperienceOrbs[allExperienceOrbs.Count - i - 1];
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
        for (int i = 0; i < amount; i++)
        {
            coin.transform.position = position;
            position = position + (Random.insideUnitCircle * resourceNodeSpawnRadius);
            position = new Vector2(Mathf.Clamp(position.x, -spawnRange.x+1.5f, spawnRange.x-1.5f), Mathf.Clamp(position.y, -spawnRange.y+1.5f, spawnRange.y-1.5f));
            coin.transform.DOJump(position, 0.5f, 1, 0.25f)
                .onComplete += () =>
            {
                coin.collider.enabled = true;
                coin.transform.DOScale(pulseSize, pulseSpeed).SetLoops(-1, LoopType.Yoyo);
            };
        }

        if (coins.CountActive > coinCap)
        {
            CleanupCoins();
        }
    }

    public void ReleaseCoin(GoldCoin coin)
    {
        if (!coin.gameObject.activeSelf) return;
        coins.Release(coin);
    }

    public void ReleaseAllCoins()
    {
        List<GoldCoin> activeCoins = new List<GoldCoin>(this.activeCoins);
        foreach (var coin in activeCoins)
        {
            ReleaseCoin(coin);
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
            GoldCoin lastCoin = allCoins[allCoins.Count - i - 1];
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