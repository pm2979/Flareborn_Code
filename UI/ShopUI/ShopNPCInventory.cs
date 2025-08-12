using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;

[System.Serializable]
public class ShopNPCInventory
{
    [SerializeField] protected Inventory shopNPC_Inventory;
    public Inventory ShopNPC_Inventory { get => shopNPC_Inventory; set => shopNPC_Inventory = value; }

    public ShopNPCInventory(Inventory inventory = null)
    {
        this.shopNPC_Inventory = inventory ?? new Inventory();
    }
}
