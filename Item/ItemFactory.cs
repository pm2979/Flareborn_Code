using static DesignEnums;

public static class ItemFactory
{
    private static ItemBaseLoader itemBaseLoader;
    private static EquipItemStatsLoader equipItemStatsLoader;
    private static ConsumableEffectsLoader consumableEffectsLoader;
    private static RuneDataLoader RuneDataLoader;

    public static void Init(ItemBaseLoader itemLoader, EquipItemStatsLoader equipItemLoader, ConsumableEffectsLoader consumableLoader, RuneDataLoader runeDataLoader)
    {
        itemBaseLoader = itemLoader;
        equipItemStatsLoader = equipItemLoader;
        consumableEffectsLoader = consumableLoader;
        RuneDataLoader = runeDataLoader;
    }

    public static ItemInstance CreateItem(int itemKey, int stack = 0, RuneStat runeStone = null)
    {
        //기본 아이템 데이터 로드
        ItemBase baseData = itemBaseLoader.GetByKey(itemKey);

        if (baseData == null)
        {
            Logger.LogError($"[ItemFactory] ItemBase not found for key: {itemKey}");
            return null;
        }

        EquipItemStats equipStats = null;
        ConsumableEffects consumableEffect = null;
        RuneData runeData = null;

        // 아이템 타입에 따라 추가 데이터 로드
        switch (baseData.ItemType)
        {
            case ItemType.Weapon:
            case ItemType.Armor:
            case ItemType.Accessory:
                equipStats = equipItemStatsLoader.GetByKey(itemKey);
                break;
            case ItemType.Consumable:
                consumableEffect = consumableEffectsLoader.GetByKey(itemKey);
                break;
            case ItemType.Rune:
                runeData = RuneDataLoader.GetByKey(itemKey);
                break;
        }

        return new ItemInstance(baseData, equipStats, consumableEffect, runeData, runeStone, stack);
    }
}
