using UnityEngine;

public class GoldCoin:Entity
{
    public uint goldValue = 1;
    public Collider2D collider;

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