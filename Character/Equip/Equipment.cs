using static DesignEnums;

public class Equipment
{
    public ItemType ItemType { get; private set; }

    public ItemInstance EquipmentItem { get; private set; }

    public Equipment(ItemType equipType) 
    { 
        ItemType = equipType;
        EquipmentItem = null;
    }

    public void Equip(ItemInstance equipment, JobType characterJobType) // 장착
    {
        if (equipment == null || ItemType != equipment.BaseData.ItemType || equipment.EquipStats == null)
        {
            return;
        }

        // 직업 타입 확인
        if (equipment.EquipStats.JobType != JobType.None && equipment.EquipStats.JobType != characterJobType)
        {
            return;
        }

        Unequip();

        EquipmentItem = equipment;
        EquipmentItem.ActiveEquipped(true);
    }

    public void Unequip() // 해제
    {
        if (EquipmentItem != null)
        {
            EquipmentItem.ActiveEquipped(false);
            EquipmentItem = null;
        }
    }
}
