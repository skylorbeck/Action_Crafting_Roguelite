using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ResourceNode.Resource targetResource = ResourceNode.Resource.Stone;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D Rb;
    public TrailRenderer trailRenderer;
}