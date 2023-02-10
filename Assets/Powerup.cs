using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : Entity
{
    public PowerupType powerupType;
    public Collider2D collider;
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        switch (powerupType)
        {
            case PowerupType.Magnet:
                ResourceManager.instance.PlayHit(hitSound);
                ResourceManager.instance.VacuumAll();
                break;
            case PowerupType.Shield://TODO
            case PowerupType.Speed://TODO
            case PowerupType.Health://TODO
            case PowerupType.Damage://TODO
            default:
                throw new ArgumentOutOfRangeException();
        }
        base.OnTriggerEnter2D(col);
        ResourceManager.instance.ReleasePowerup(this);
    }

    public enum PowerupType
    {
        Magnet,
        Shield,
        Speed,
        Health,
        Damage,
    }

    public void SetPowerup(PowerupType powerup)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = ResourceManager.instance.powerupSprites[(int) powerup]; 
        powerupType = powerup;
    }
}
