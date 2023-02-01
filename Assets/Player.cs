using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    public static Player instance;

    [SerializeReference] RunStats runStats;

    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected uint experience = 0;
    [SerializeField] protected uint goldCoins = 0;
    [SerializeField] protected uint experienceToNextLevel = 100;
    [SerializeField] protected uint level = 0;

    [SerializeField] protected float invincibleTime = 1f;
    [SerializeField] protected float invincibleTimer = 0f;

    [SerializeField] protected Image experienceBar;
    [SerializeField] protected TextMeshProUGUI levelText;
    [SerializeField] protected TextMeshProUGUI goldText;
    
    [SerializeField] protected Button resumeButton;//TODO move this somewhere else
    protected override void Start()
    {
        instance = this;
        base.Start();
    }

    private void Awake()
    {
        health = maxHealth;
        HealthDisplay.instance.SetMaxHealth(maxHealth);
        HealthDisplay.instance.SetHealth(health);
        Rb.freezeRotation = PlayerPrefs.GetInt("sillyMode", 0) == 0;
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

        experienceBar.rectTransform.sizeDelta = new Vector2(Mathf.Lerp(experienceBar.rectTransform.sizeDelta.x,(experience / (float)experienceToNextLevel) * 1820,Time.fixedDeltaTime), 64);

        base.FixedUpdate();
    }

    public void TakeDamage(int damage)
    {
        spriteRenderer.DOColor(Color.red, 0.1f).SetLoops(8, LoopType.Yoyo).onComplete += () => spriteRenderer.DOColor(Color.white, 0.1f);
        // transform.DOShakeScale(1f, 0.5f);
        health -= damage;
        HealthDisplay.instance.SetHealth(health);
        if (health <= 0)
        {
            Kill();
        }
    }

    private void Kill()
    {
        ExplosionManager.instance.SpawnExplosion(transform.position);
        EnemyManager.instance.ReleaseAllEnemies();
        ResourceManager.instance.ReleaseAllAll();
        gameObject.SetActive(false); //TODO replace this 
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
                runStats.stoneCollected += amount; //TODO display on screen?
                //TODO add to inventory at end of run
                break;
            case ResourceDrop.Resource.Wood:
                runStats.woodCollected += amount;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resource), resource, null);
        }
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
        CheckForLevelUp();
    }

    private void CheckForLevelUp()
    {
        if (experience >= experienceToNextLevel)
        {
            experience -= experienceToNextLevel;
            level++;
            experienceToNextLevel = (uint)(experienceToNextLevel * 1.5f);
            maxHealth += 10;
            health = maxHealth;
            levelText.text = "Lv. " + level;
            
            //TODO hand off to level up system
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0f, 0.5f).SetUpdate(true).onComplete += () =>
            {
                resumeButton.interactable = true;
            };
            resumeButton.gameObject.SetActive(true);
            resumeButton.transform.DOLocalMove(Vector3.zero, 0.5f).SetUpdate(true).SetEase(Ease.OutBack);
        }
    }

    public void ResumeGame()//TODO move this to the level up system
    {
        resumeButton.interactable = false;
        Time.timeScale = 1f;
        resumeButton.transform.DOLocalMove(new Vector3(0, -1000, 0), 0.5f).SetUpdate(true).SetEase(Ease.InBack).onComplete += () =>
            {
                resumeButton.gameObject.SetActive(false);
                CheckForLevelUp();
            };
    }

    public void AddCoin(uint goldValue)
    {
        goldCoins += goldValue;
        goldText.text = "x" + goldCoins;
    }
}

