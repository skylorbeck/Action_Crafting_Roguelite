using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceOrb : Entity
{
   public uint experienceValue = 1;
   public Collider2D collider;
   protected override void Awake()
   {
      UpdateSprite();
        base.Awake();
   }

   protected override void OnTriggerEnter2D(Collider2D col)
   {
      collider.enabled = false;
      if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
      {
         Player.instance.AddExperience(experienceValue);
         ResourceManager.instance.PlayHit(hitSound);
         magnetToPlayer = false;
         ResourceManager.instance.ReleaseExperienceOrb(this);
      }
   }

   public void UpdateSprite()
   {
      spriteRenderer.sprite = experienceValue switch
      {
         > 1 and < 5 => sprites[1],
         > 5 and < 10 => sprites[2],
         > 10 and < 20 => sprites[3],
         > 20 and < 50 => sprites[4],
         > 50 and < 100 => sprites[5],
         > 100 and < 200 => sprites[6],
         > 200 and < 500 => sprites[7],
         > 500 => sprites[8],
         _ => sprites[0],
      };
   }

   public void SetAmount(uint amount)
   {
      experienceValue = amount;
      UpdateSprite();
   }
   
   public uint GetAmount()
   {
      return experienceValue;
   }
}
