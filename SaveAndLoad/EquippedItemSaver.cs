// EquippedItemSaver.cs
using UnityEngine;
using System.Collections.Generic;
using static DesignEnums;
using System;

public class EquippedItemSaver : IDataPersistence
{
    private PartyManager partyManager;

    // 필요한 장비 슬롯만 저장
    private static readonly ItemType[] EquippedSlotTypes = new ItemType[]
    {
        ItemType.Weapon,
        ItemType.Armor,
        ItemType.Accessory
    };

    public EquippedItemSaver(PartyManager partyManager)
    {
        this.partyManager = partyManager;
    }

    public void SaveData(ref GameData data)
    {
        if (partyManager == null || partyManager.Party == null)
        {
            Debug.LogWarning("[EquipmentSaver] PartyManager 또는 Party가 null입니다.");
            return;
        }

        data.savedEquipments.Clear();

        foreach (var character in partyManager.Party.Members)
        {
            var saved = new SavedEquippedItemData
            {
                characterID = character.ID,
                equippedItemKeys = new List<int>()
            };

            foreach (ItemType type in EquippedSlotTypes)
            {
                var item = character.EquipmentController.GetEquippedItem(type);
                saved.equippedItemKeys.Add(item != null ? item.ItemKey : -1);
            }

            data.savedEquipments.Add(saved);

            Debug.Log($"[EquipmentSaver] {character.Name}의 장비 저장 완료 (무기/방어구/장신구만).");
        }
    }

    public void LoadData(GameData data)
    {
        // 나중에 구현
    }

}