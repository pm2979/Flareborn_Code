using System;
using UnityEngine;
using static DesignEnums;

[Serializable]
public class ItemInstance
{
    public int ItemKey { get; private set; }
    public ItemBase BaseData { get; private set; }
    public EquipItemStats EquipStats { get; private set; }
    public ConsumableEffects ConsumableEffect { get; private set; }
    public RuneData RuneData { get; private set; }

    [field:SerializeField] public string Name { get; private set; }
    [field:SerializeField] public int CurrentStack { get; private set; }

    public RuneStat RuneStat { get; private set; }

    public bool IsEquipped { get; private set; }

    public ItemInstance(ItemBase baseData, EquipItemStats equipStats = null, ConsumableEffects effect = null, RuneData runeData = null, RuneStat runeStat = null, int stack = 0)
    {
        if (baseData == null)
        {
            throw new ArgumentNullException(nameof(baseData), "ItemInstance를 생성하려면 ItemBase 데이터가 반드시 필요합니다.");
        }

        BaseData = baseData;
        ItemKey = baseData.key;
        Name = baseData.Name;

        if (BaseData.ItemType == ItemType.Weapon || BaseData.ItemType == ItemType.Armor || BaseData.ItemType == ItemType.Accessory)
            EquipStats = equipStats;

        if (BaseData.ItemType == ItemType.Consumable)
            ConsumableEffect = effect;

        if (BaseData.ItemType == ItemType.Rune)
        {
            RuneData = runeData;
            RuneStat = runeStat ?? new RuneStat();
        }

        CurrentStack = Mathf.Clamp(stack, 0, BaseData.MaxStack);
        IsEquipped = false;
    }

    public void AddStack(int amount)
    {
        int newStack = CurrentStack + amount;
        CurrentStack = Mathf.Clamp(newStack, 0, BaseData.MaxStack);
    }

    public void SetStack(int amount)
    {
        CurrentStack = Mathf.Clamp(amount, 0, BaseData.MaxStack);
    }

    public void Use()
    {
        if (!BaseData.IsUsable || CurrentStack <= 0)
            return;

        CurrentStack--;
    }

    public void ActiveEquipped(bool isEquipped)
    {
        IsEquipped = isEquipped;
    }

    public void RemoveStack(int amount)
    {
        CurrentStack = Mathf.Max(0, CurrentStack - amount);
    }

    public string ItemName()
    {
        switch(BaseData.ItemType)
        {
            case ItemType.Rune:
                return $"{BaseData.Name} +{RuneStat.Value}";
            default:
                return BaseData.Name;
        }
    }

}

