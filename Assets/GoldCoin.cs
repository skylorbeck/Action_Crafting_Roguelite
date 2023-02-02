using UnityEngine;

public class GoldCoin:Entity
{
    public uint goldValue = 1;
    public Collider2D collider;
    public void Awake()
    {
        Rb.freezeRotation = PlayerPrefs.GetInt("sillyMode", 0) == 0;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        collider.enabled = false;
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player.instance.AddCoin(goldValue);
            ResourceManager.instance.ReleaseCoin(this);
        }
    }

    public void SetAmount(uint amount)
    {
        goldValue = amount;
    }
    
    public uint GetAmount()
    {
        return goldValue;
    }
}