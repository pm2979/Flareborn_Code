using UnityEngine;
using System.Collections.Generic;

public class InventorySaver : IDataPersistence
{
    private Inventory inventory;

    public InventorySaver(Inventory inventory)
    {
        this.inventory = inventory;

        if (inventory == null)
        {
            UnityEngine.Debug.LogWarning("[InventorySaver] 생성자에서 Inventory가 null입니다.");
        }
    }

    public void SaveData(ref GameData data)
    {
        if (inventory == null)
        {
            Debug.LogWarning("[InventorySaver] Inventory가 null이라 저장 생략");
            return;
        }

        data.savedItems.Clear();

        foreach (var itemType in System.Enum.GetValues(typeof(DesignEnums.ItemType)))
        {
            // None 타입은 저장하지 않음
            if ((DesignEnums.ItemType)itemType == DesignEnums.ItemType.None)
                continue;

            var items = inventory.GetItems((DesignEnums.ItemType)itemType);
            foreach (var item in items)
            {
                var savedItem = new SavedItemData
                {
                    itemKey = item.ItemKey,
                    currentStack = item.CurrentStack,
                    runeValue = item.RuneStat?.Value ?? 0
                };
                data.savedItems.Add(savedItem);
            }
        }

        Debug.Log($"[저장] 인벤토리 아이템 {data.savedItems.Count}개 저장됨");
    }

    public void LoadData(GameData data)
    {
        //if (inventory == null)
        //{
        //    Debug.LogWarning("[InventorySaver] Inventory가 null이라 불러오기 생략");
        //    return;
        //}

        //if (data.savedItems == null) return;

        //foreach (var savedItem in data.savedItems)
        //{
        //    var item = ItemFactory.CreateItem(savedItem.itemKey, savedItem.currentStack, new RuneStat(savedItem.runeValue));
        //    inventory.AddItem(item);
        //}

        //Debug.Log($"[불러오기] 인벤토리 아이템 {data.savedItems.Count}개 로드됨");
    }
}
