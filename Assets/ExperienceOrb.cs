using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceOrb : Entity
{
   public uint experienceValue = 1;
   public Collider2D collider;
   public void Awake()
   {
      UpdateSprite();
      collider.enabled = false;
      Rb.freezeRotation = PlayerPrefs.GetInt("sillyMode", 0) == 0;

   }

   protected override void OnTriggerEnter2D(Collider2D col)
   {
      if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
      {
         Player.instance.AddExperience(experienceValue);
         ResourceManager.instance.ReleaseExperienceOrb(this);
      }
   }

   public void UpdateSprite()
   {
      spriteRenderer.sprite = experienceValue switch
      {
         > 1 and < 4 => sprites[1],
         > 4 and < 8 => sprites[2],
         > 8 and < 16 => sprites[3],
         > 16 and < 32 => sprites[4],
         > 32 and < 64 => sprites[5],
         > 64 and < 128 => sprites[6],
         > 128 and < 256 => sprites[7],
         > 256 and < 512 => sprites[8],
         >512 => sprites[9],
         _ => sprites[0],
      };
   }

   public void SetAmount(uint amount)
   {
      experienceValue = amount;
      UpdateSprite();
   }
}
