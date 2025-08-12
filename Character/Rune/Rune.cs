using static DesignEnums;

// 개별 룬 슬롯을 관리하는 클래스
public class Rune
{
    // 이 슬롯에 할당된 능력치 타입
    public AbilityType AbilityType { get; private set; }

    // 슬롯에 장착된 룬 아이템
    public ItemInstance RuneItem { get; private set; }

    public Rune(AbilityType abilityType)
    {
        AbilityType = abilityType;
        RuneItem = null;
    }

    public void Equip(ItemInstance runeToEquip) // 룬 장착
    {
        if (runeToEquip == null || runeToEquip.RuneStat == null || runeToEquip.RuneStat.AbilityType != this.AbilityType)
        {
            return;
        }

        Unequip();

        RuneItem = runeToEquip;

        RuneItem.ActiveEquipped(true);
    }

    public void Unequip() //룬 해제
    {
        if (RuneItem != null)
        {
            RuneItem.ActiveEquipped(false);
            RuneItem = null;
        }
    }
}
