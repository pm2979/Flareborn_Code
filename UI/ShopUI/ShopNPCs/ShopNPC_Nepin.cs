using UnityEngine;

public class ShopNPC_Nepin : ShopNPC
{
    protected override void ShopInit()
    {
        // 상점 NPC 네핀이 판매하는 아이템들
        base.ShopInit();
        shopNPCInventory.ShopNPC_Inventory.AddItem(ItemFactory.CreateItem(1001, 5));
        shopNPCInventory.ShopNPC_Inventory.AddItem(ItemFactory.CreateItem(2001));
        shopNPCInventory.ShopNPC_Inventory.AddItem(ItemFactory.CreateItem(2101)); // 나무활
        shopNPCInventory.ShopNPC_Inventory.AddItem(ItemFactory.CreateItem(2002));
        shopNPCInventory.ShopNPC_Inventory.AddItem(ItemFactory.CreateItem(4001));
        shopNPCInventory.ShopNPC_Inventory.AddItem(ItemFactory.CreateItem(4001));
        shopNPCInventory.ShopNPC_Inventory.AddItem(ItemFactory.CreateItem(4002));
        shopNPCInventory.ShopNPC_Inventory.AddItem(ItemFactory.CreateItem(5001));
        shopNPCInventory.ShopNPC_Inventory.AddItem(ItemFactory.CreateItem(5002));
        shopNPCInventory.ShopNPC_Inventory.AddItem(ItemFactory.CreateItem(6001));
        shopNPCInventory.ShopNPC_Inventory.AddItem(ItemFactory.CreateItem(6002));
    }
}
