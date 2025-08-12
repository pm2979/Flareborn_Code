using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DesignEnums;

[Serializable]
public class ItemSlotList
{
    public ItemSlotList(ItemType type) 
    { 
        itemType = type;
    }

    public ItemType itemType;
    public List<ItemInstance> items = new List<ItemInstance>();
}

[Serializable]
public class Inventory
{
    [SerializeField] private List<ItemSlotList> itemSlotLists;

    public Action<List<ItemInstance>, ItemType> UIUpdate;

    public Inventory()
    {
        // 각 아이템 타입에 맞게 리스트를 초기화합니다.
        itemSlotLists = new List<ItemSlotList>();
        foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
        {
            if (type != ItemType.None)
            {
                itemSlotLists.Add(new ItemSlotList(type));
            }
        }
    }

    // ItemType에 해당하는 아이템 리스트를 반환
    private List<ItemInstance> GetSlotsByType(ItemType type)
    {
        var list = itemSlotLists.FirstOrDefault(slotList => slotList.itemType == type);
        if (list == null)
        {
            Logger.LogError($"[Inventory] ItemType '{type}'에 해당하는 슬롯 리스트가 없습니다.");
            return new List<ItemInstance>();
        }
        return list.items;
    }

    // 아이템 추가
    public bool AddItem(ItemInstance item, int amount = 1)
    {
        if (item == null || amount <= 0)
            return false;

        bool success = (item.BaseData.MaxStack == 1) ? TryAddUnstackable(item, amount) : TryAddStackable(item, amount);

        UIUpdate?.Invoke(GetSlotsByType(item.BaseData.ItemType), item.BaseData.ItemType);
        return success;
    }

    // 스택이 불가능한 아이템
    private bool TryAddUnstackable(ItemInstance item, int amount)
    {
        List<ItemInstance> slots = GetSlotsByType(item.BaseData.ItemType);
        int added = 0;

        for (int i = 0; i < amount; i++)
        {
            RuneStat runeStone = null;

            // Rune 타입이라면 RuneStone을 새로 생성
            if (item.BaseData.ItemType == ItemType.Rune)
            {
                runeStone = item.RuneStat;
            }

            // 새로운 인스턴스 생성
            ItemInstance newItem = ItemFactory.CreateItem(item.ItemKey, 1, runeStone);
            if (newItem == null)
            {
                continue;
            }

            slots.Add(newItem);
            added++;
        }

        return added == amount;
    }


    // 스택 가능한 아이템 추가
    private bool TryAddStackable(ItemInstance item, int amount)
    {
        List<ItemInstance> slots = GetSlotsByType(item.BaseData.ItemType);
        int remaining = amount;

        foreach (ItemInstance slot in slots)
        {
            if (slot.ItemKey != item.ItemKey || slot.CurrentStack >= slot.BaseData.MaxStack)
                continue;

            int space = slot.BaseData.MaxStack - slot.CurrentStack;
            int toAdd = Mathf.Min(space, remaining);
            slot.AddStack(toAdd);
            remaining -= toAdd;

            if (remaining <= 0) break;
        }

        while (remaining > 0)
        {
            int toAdd = Mathf.Min(item.BaseData.MaxStack, remaining);
            ItemInstance newItem = ItemFactory.CreateItem(item.ItemKey);
            if (newItem == null)
            {
                Logger.LogError($"[Inventory] ItemFactory 생성 실패: {item.ItemKey}");
                break;
            }

            newItem.SetStack(toAdd);
            slots.Add(newItem);
            remaining -= toAdd;
        }

        if (remaining > 0)
        {
            Logger.LogError($"[Inventory] {remaining}개의 아이템이 인벤토리에 추가되지 못함 (공간 부족)");
            return false;
        }

        return true;
    }

    // 특정 타입의 슬롯 목록 조회
    public List<ItemInstance> GetItems(ItemType type)
    {
        return GetSlotsByType(type);
    }

    public bool RemoveItem(ItemInstance item, int amount = 1)
    {
        if (item == null || amount <= 0)
            return false;

        var slots = GetSlotsByType(item.BaseData.ItemType);
        bool removed = false;

        if (item.BaseData.MaxStack == 1) // 비스택형
        {
            ItemInstance itemToRemove = slots.FirstOrDefault(slot => slot == item || slot.ItemKey == item.ItemKey);

            if (itemToRemove != null)
            {
                slots.Remove(itemToRemove);
                removed = true;
            }
            else
            {
                Logger.LogError($"[Inventory] 인벤토리에서 아이템({item.BaseData.Name})을 찾지 못해 제거할 수 없습니다.");
            }
        }
        else // 스택형
        {
            int amountLeftToRemove = amount;

            for (int i = slots.Count - 1; i >= 0; i--)
            {
                var currentSlot = slots[i];
                // 같은 종류의 아이템 확인
                if (currentSlot.ItemKey == item.ItemKey)
                {
                    if (currentSlot.CurrentStack > amountLeftToRemove)
                    {
                        // 현재 슬롯의 스택이 제거할 양보다 많으면, 스택을 줄이고 종료합니다.
                        currentSlot.RemoveStack(amountLeftToRemove);
                        amountLeftToRemove = 0;
                    }
                    else
                    {
                        // 현재 슬롯의 스택을 전부 제거해도 부족하거나 같은 경우, 슬롯을 완전히 제거합니다.
                        amountLeftToRemove -= currentSlot.CurrentStack;
                        slots.RemoveAt(i);
                    }
                }

                // 필요한 수량만큼 모두 제거했으면 반복 중단
                if (amountLeftToRemove <= 0) break;
            }

            // 요청된 수량만큼 성공적으로 제거되었는지 확인합니다.
            if (amountLeftToRemove <= 0)
            {
                removed = true;
            }
            else
            {
                // 인벤토리에 아이템이 부족했던 경우
                removed = false;
            }
        }

        if (removed)
        {
            UIUpdate?.Invoke(slots, item.BaseData.ItemType);
        }

        return removed;
    }

    public bool DoesItemExist(ItemInstance item)
    {
        // # 방어로직
        if (item == null) return false;

        // 아이템 타입에 해당하는 슬롯 리스트 가져옴
        var slots = GetSlotsByType(item.BaseData.ItemType);

        // 비스택형 : 개별 인스턴스 존재 여부 확인
        if (item.BaseData.MaxStack == 1)
        {
            // 체크할 아이템이 슬롯에 존재하는지 확인
            return slots.Any(slot => slot == item || slot.ItemKey == item.ItemKey);
        }

        // 스택형 : 총 수량이 0보다 큰지 확인
        foreach (var slot in slots)
        {
            if (slot.ItemKey == item.ItemKey && slot.CurrentStack > 0) return true;
        }

        return false;
    }
}