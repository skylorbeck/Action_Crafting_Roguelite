using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using PerkSystem;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : Entity, IDamageable
{
    public static Player instance;
    [SerializeField] public ClassRegistry classRegistry;
    [SerializeField] public ToolRegistry toolRegistry;
    // [SerializeField] public int classIndex = 0;
    // [SerializeField] public int weaponIndex = 0;//unused
    [SerializeField] protected bool spawnWithWeapons = true;
    [SerializeField] protected Transform weaponHolder;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] RunStats runStats;
    
    [SerializeField] PerkStatModifiers perkStatModifiers;
    [SerializeField] public List<Perk> equippedPerks = new List<Perk>();
    [SerializeField] public List<Tool> equippedTools = new List<Tool>();
    [SerializeField] protected int maxHealth = 6;
    [SerializeField] protected uint experience = 0;
    [SerializeField] protected float experienceScale = 1.1f;
    [SerializeField] protected uint experienceToNextLevel = 100;
    [SerializeField] protected uint level = 0;

    [SerializeField] protected float invincibleTime = 1f;
    [SerializeField] protected float invincibleTimer = 0f;
    
    [SerializeField] protected Collider2D pickupCircle;
    [SerializeField] protected Image experienceBar;
    [SerializeField] protected TextMeshProUGUI levelText;
    [SerializeField] protected TextMeshProUGUI goldText;
    [SerializeField] protected TextMeshProUGUI stoneText;
    [SerializeField] protected TextMeshProUGUI woodText;
    [SerializeField] protected SpriteAnimator spriteAnimator;

    [SerializeField] protected PlayerInput playerInput;
    
    protected override IEnumerator Start()
    {
        instance = this;
        audioSource.volume = PlayerPrefs.GetFloat("effectVolume", 1);
        yield return new WaitUntil(() => SaveManager.instance != null);
        yield return new WaitUntil(() => SaveManager.instance.loaded);
        SetClass(classRegistry.GetClass((int)SaveManager.instance.GetPlayerToolData().GetEquippedType()));
        if (spawnWithWeapons)
        {
            EquipTool();
        }
        yield return base.Start();
    }

    public uint GetGold()
    {
        return runStats.goldCollected;
    }
    
    public void EquipTool(Tool tool = null)
    {
        if (tool == null)
        {
            tool = classRegistry.GetToolsOfClass((int)SaveManager.instance.GetPlayerToolData().GetEquippedType())[0];
        }
        
        if (tool != null)
        {
            Tool newTool;
            equippedTools.Add(newTool = Instantiate(tool, weaponHolder));
            newTool.toolStats = SaveManager.instance.GetPlayerToolData().GetEquipped();
        }
    }

    private void SetClass(ClassObject classObject)
    {
        sprites = classObject.classSprites;
        spriteAnimator.SetSprites(sprites);
        spriteRenderer.sprite = sprites[0];
    }

    protected override void Awake()
    {
        health = maxHealth;
        HealthDisplay.instance.SetMaxHealth(maxHealth);
        HealthDisplay.instance.SetHealth(health);
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (invincibleTimer > 0)
        {
            invincibleTimer -= Time.fixedDeltaTime;
        }

        // experienceBar.rectTransform.sizeDelta = new Vector2(Mathf.Lerp(experienceBar.rectTransform.sizeDelta.x,(experience / (float)experienceToNextLevel) * 1820,Time.fixedDeltaTime), 64);

        base.FixedUpdate();
    }

    public bool TakeDamage(int damage)
    {
        PlayHit();
        spriteRenderer.DOColor(Color.red, 0.1f).SetLoops(8, LoopType.Yoyo).onComplete += () => spriteRenderer.DOColor(Color.white, 0.1f);
        // transform.DOShakeScale(1f, 0.5f);
        health -= damage;
        HealthDisplay.instance.SetHealth(health);
        if (health <= 0)
        {
            Kill();
            return true;
        }

        return false;
    }

    public void Kill()
    {
        ExplosionManager.instance.SpawnExplosion(transform.position);
        EnemyManager.instance.ReleaseAllEnemies();
        EnemyManager.instance.SetSpawnEnemies(false);
        ResourceManager.instance.ReleaseAllAll();
        ResourceManager.instance.SetSpawnResources(false);
        spriteRenderer.color = Color.clear;
        SetCanMove(false);
        weaponHolder.gameObject.SetActive(false);
        TimerManager.instance.isPaused = true;
        StartCoroutine(GameOverManager.instance.GameOver());
    }


    protected override void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy") && invincibleTimer <= 0)
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            TakeDamage(enemy.GetPower());
            invincibleTimer = invincibleTime;
        }

        base.OnCollisionEnter2D(col);
    }

    protected override void OnCollisionStay2D(Collision2D col)
    {
        OnCollisionEnter2D(col);
        base.OnCollisionStay2D(col);
    }

    public void AddEnemyKill()
    {
        runStats.enemiesKilled++;
    }

    public void AddResource(ResourceDrop.Resource resource, uint amount)
    {
        runStats.resourcesCollected += amount;
        switch (resource)
        {
            case ResourceDrop.Resource.Stone:
                runStats.stoneCollected += amount; 
                PopupManager.instance.SpawnStoneNumber((int)amount);
                break;
            case ResourceDrop.Resource.Wood:
                runStats.woodCollected += amount;
                PopupManager.instance.SpawnWoodNumber((int)amount);
                break;
            case ResourceDrop.Resource.Iron:
            case ResourceDrop.Resource.Gold:
            case ResourceDrop.Resource.Diamond:
            default:
                throw new ArgumentOutOfRangeException(nameof(resource), resource, null);
        }
        UpdateResourceText();
    }

    public void AddNodeHarvest(ResourceNode.Resource resource)
    {
        switch (resource)
        {
            case ResourceNode.Resource.Stone:
                runStats.stoneNodesHarvested++;
                break;
            case ResourceNode.Resource.Wood:
                runStats.woodNodesHarvested++;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resource), resource, null);
        }
    }

    public void AddExperience(uint experienceValue)
    {
        experience += experienceValue;
        PopupManager.instance.SpawnExpNumber((int)experienceValue);
        CheckForLevelUp();
    }

    public void CheckForLevelUp()
    {
        if (experience >= experienceToNextLevel)
        {
            experience -= experienceToNextLevel;
            level++;
            experienceToNextLevel = (uint)Mathf.RoundToInt(experienceToNextLevel * experienceScale);
            invincibleTimer = invincibleTime;
            levelText.text = "Lv. " + level;
            
            PerkManager.instance.ShowPerkMenu();
            
        }
        DOTween.Kill(experienceBar.rectTransform);
        DOTween.To(() => experienceBar.rectTransform.sizeDelta.x, x => experienceBar.rectTransform.sizeDelta = new Vector2(x, experienceBar.rectTransform.sizeDelta.y), (experience / (float)experienceToNextLevel) * 1820, 0.5f);

    }

    public void AddCoin(uint goldValue)
    {
        runStats.goldCollected += goldValue;
        UpdateCoinText();
        PopupManager.instance.SpawnCoinNumber((int)goldValue);
    }

    private void UpdateCoinText()
    {
        goldText.text = "x" + runStats.goldCollected;
    }

    public void SetCanMove(bool b)
    {
        playerInput.enabled = b;
    }

    public void UpdateClass()
    {
        SetClass(classRegistry.GetClass((int)SaveManager.instance.GetPlayerToolData().GetEquippedType()));
    }
    
    public void AddPerk(Perk perk)
    {
        AddPerkStatModifiers(perk.perkStatModifiers);
        equippedPerks.Add(perk);
    }
    
    public void RemovePerk(Perk perk)
    {
        RemovePerkStatModifiers(perk.perkStatModifiers);
        equippedPerks.Remove(perk);
    }

    private void AddPerkStatModifiers(PerkStatModifiers newPerkStatModifiers)
    {
        perkStatModifiers.Add(newPerkStatModifiers);
    }
    
    private void RemovePerkStatModifiers(PerkStatModifiers newPerkStatModifiers)
    {
        perkStatModifiers.Remove(newPerkStatModifiers);
    }
    
    public PerkStatModifiers GetPerkStatModifiers()
    {
        return perkStatModifiers;
    }

    public void AddHealth(int healthFlatBonus)
    {
        maxHealth += healthFlatBonus;
        HealthDisplay.instance.SetMaxHealth(maxHealth);
        health += healthFlatBonus;
        HealthDisplay.instance.SetHealth(health);
    }
    
    public int GetDamage()
    {
        return Mathf.RoundToInt((1 + perkStatModifiers.damageFlatBonus * (perkStatModifiers.damageMultiplierBonus + 1))*Random.Range(0.9f, 1.1f));
    }

    public float GetAttackSpeedBonus()
    {
        return 1 + perkStatModifiers.attackSpeedMultiplierBonus + equippedTools.Sum(tool => tool.toolStats.attackSpeedBonus);
    }
    
    public float GetMoveSpeedBonus()
    {
        return 1 + perkStatModifiers.movementSpeedMultiplierBonus + equippedTools.Sum(tool => tool.toolStats.movementSpeedBonus);
    }
    
    public float GetCritChance()
    {
        return 0.1f + perkStatModifiers.critChanceFlatBonus + equippedTools.Sum(tool => tool.toolStats.critChanceBonus);
    }
    
    public float GetCritDamageBonus()
    {
        return 1 + perkStatModifiers.critDamageMultiplierBonus +  equippedTools.Sum(tool => tool.toolStats.critDamageBonus);
    }
    
    public float GetProjectileSpeedBonus()
    {
        return 1 + perkStatModifiers.projectileSpeedMultiplierBonus + equippedTools.Sum(tool => tool.toolStats.projectileSpeedBonus);
    }
    
    public float GetProjectileSizeBonus()
    {
        return 1 + perkStatModifiers.projectileSizeMultiplierBonus + equippedTools.Sum(tool => tool.toolStats.projectileSizeBonus);
    }
    
    public int GetExtraProjectiles()
    {
        return perkStatModifiers.projectileCountFlatBonus + equippedTools.Sum(tool => tool.toolStats.projectileCountBonus);
    }
    
    public float GetAoERadius()
    {
        return (1 + perkStatModifiers.areaOfEffectMultiplierBonus +
                equippedTools.Sum(tool => tool.toolStats.areaOfEffectBonus));
    }

    public float GetExperienceBonus()
    {
        return 1+ perkStatModifiers.experienceValueMultiplierBonus;
    }
    
    public float GetGoldBonus()
    {
        return 1 + perkStatModifiers.goldValueMultiplierBonus;
    }
    
    public int GetResourceBonus()
    {
        return perkStatModifiers.resourceDropFlatBonus;
    }
    
    public bool PicksSpawnOnDeath()
    {
        return perkStatModifiers.enemiesSpawnPick;
    }

    public bool NodesExplode()
    {
        return perkStatModifiers.stoneNodesExplode;
    }

    public void UpdatePickupSize()
    {
        pickupCircle.transform.localScale = Vector3.one * GetAoERadius() *2f;
    }
    
    public float GetEnemyHealthMultiplier()
    {
        return 1 + perkStatModifiers.enemyHealthMultiplier;
    }
    
    public float GetEnemySpeedMultiplier()
    {
        return 1 + perkStatModifiers.enemySpeedMultiplier;
    }
    
    public bool SplitPicks()
    {
        return perkStatModifiers.splitPicks;
    }

    public bool SplitAxes()
    {
        return perkStatModifiers.splitAxes;
    }

    public bool CombineAxes()
    {
        return perkStatModifiers.combineAxes;
    }

    public bool TreesBurn()
    {
        return perkStatModifiers.treesBurn;
    }

    public RunStats GetRunStats()
    {
        return runStats;
    }

    public void SetMetaStats(RunStats getMetaStats)
    {
        runStats = getMetaStats;
        UpdateCoinText();
        UpdateResourceText();
    }
    
    public bool SpendResource(ResourceNode.Resource resource, uint amount)
    {
        bool success = false;
        switch (resource)
        {
            case ResourceNode.Resource.Stone:
                if (runStats.stoneCollected >= amount)
                {
                    runStats.stoneCollected -= amount;
                    success = true;
                }
                break;
            case ResourceNode.Resource.Wood:
                if (runStats.woodCollected >= amount)
                {
                    runStats.woodCollected -= amount;
                    success = true;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resource), resource, null);
        }
        UpdateResourceText();
        return success;
    }
    
    public bool CanAfford(uint amount)
    {
        return runStats.goldCollected >= amount;
    }
    
    public bool SpendGold(uint amount)
    {
        if (runStats.goldCollected >= amount)
        {
            runStats.goldCollected -= amount;
            UpdateCoinText();
            return true;
        }

        return false;
    }
    
    public void UpdateResourceText()
    {
        stoneText.text = "x" + runStats.stoneCollected;
        woodText.text = "x" + runStats.woodCollected;
    }
    
    protected virtual void PlayHit()
    {
        audioSource.PlayOneShot(hitSound);
    }
}

