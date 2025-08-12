using System;
using System.Linq;
using System.Collections.Generic;
using static DesignEnums;

[Serializable]
public class EquipmentController
{
    // 장비 슬롯을 관리하는 리스트
    private readonly List<Equipment> equipmentSlots;

    // 장비 상태가 변경될 때 호출되는 이벤트
    public event Action<ItemType> OnEquipmentChanged;

    public EquipmentController()
    {
        equipmentSlots = new List<Equipment>();

        foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
        {
            equipmentSlots.Add(new Equipment(type));
        }
    }

    // 장비 장착
    public void Equip(ItemInstance itemToEquip, JobType characterJobType)
    {
        if (itemToEquip?.BaseData == null) return;

        ItemType type = itemToEquip.BaseData.ItemType;
        Equipment slot = equipmentSlots.Find(s => s.ItemType == type);

        if (slot != null)
        {
            slot.Equip(itemToEquip, characterJobType);
            OnEquipmentChanged?.Invoke(type); // 이벤트 호출
        }
    }

    // 장비를 해제
    public void Unequip(ItemType type)
    {
        Equipment slot = equipmentSlots.Find(s => s.ItemType == type);
        if (slot != null && slot.EquipmentItem != null)
        {
            slot.Unequip();
            OnEquipmentChanged?.Invoke(type); // 이벤트 호출
        }
    }

    // 슬롯에 장착된 아이템 리턴
    public ItemInstance GetEquippedItem(ItemType type)
    {
        Equipment slot = equipmentSlots.Find(s => s.ItemType == type);
        if (slot != null)
        {
            return slot.EquipmentItem;
        }

        return null;
    }

    public EquipItemStats GetTotalEquippedStats() // 현재 장착된 모든 장비의 능력치 총합을 계산하여 반환합니다.
    {
        IEnumerable<EquipItemStats> equippedStatsList = equipmentSlots.Where(slot => slot.EquipmentItem?.EquipStats != null).Select(slot => slot.EquipmentItem.EquipStats);

        // 각 스탯의 총합을 계산하여 새로운 객체로 반환
        return new EquipItemStats
        {
            HP = equippedStatsList.Sum(stats => stats.HP),
            ATK = equippedStatsList.Sum(stats => stats.ATK),
            SATK = equippedStatsList.Sum(stats => stats.SATK),
            DEF = equippedStatsList.Sum(stats => stats.DEF),
            SDEF = equippedStatsList.Sum(stats => stats.SDEF),
            SPD = equippedStatsList.Sum(stats => stats.SPD),
            Critical = equippedStatsList.Sum(stats => stats.Critical),
            EVA = equippedStatsList.Sum(stats => stats.EVA)
        };
    }
}
