using System;
using System.Collections.Generic;
using System.Linq;
using static DesignEnums;

// 모든 룬 슬롯을 관리하는 컨트롤러
[Serializable]
public class RuneController
{
    // 모든 능력치 타입에 대한 룬 슬롯 리스트
    private readonly List<Rune> runeSlots;

    public IReadOnlyList<Rune> RuneSlots => runeSlots;

    // 룬 장착/해제 시 발생하는 이벤트
    public event Action<int> OnRuneChanged;

    public RuneController(CharacterInstance character)
    {
        runeSlots = new List<Rune>();

        // 모든 AbilityType에 대해 슬롯 생성
        foreach (AbilityType type in Enum.GetValues(typeof(AbilityType)))
        {
            if (type == AbilityType.None || type == AbilityType.FS) continue;
            runeSlots.Add(new Rune(type));
        }
        
        // 특정 AbilityType 추가 생성
        if(character.IsFlare)
        {
            runeSlots.Add(new Rune(AbilityType.FS));
        }
        else
        {
            runeSlots.Add(new Rune(character.CharacterData.AbilityType));
        }
    }

    // 룬을 장착
    public void Equip(int slotIndex, ItemInstance runeToEquip)
    {
        // 유효하지 않은 인덱스이거나, 장착할 룬이 없으면 반환
        if (slotIndex < 0 || slotIndex >= runeSlots.Count || runeToEquip?.RuneStat == null) return;

        Rune slot = runeSlots[slotIndex];

        // 슬롯의 능력치 타입과 룬의 능력치 타입이 일치하는지 확인
        if (slot.AbilityType == runeToEquip.RuneStat.AbilityType)
        {
            slot.Equip(runeToEquip);
            OnRuneChanged?.Invoke(slotIndex);
        }
    }

    // 룬을 해제
    public void Unequip(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= runeSlots.Count) return;

        Rune slot = runeSlots[slotIndex];
        if (slot.RuneItem != null)
        {
            slot.Unequip();
            OnRuneChanged?.Invoke(slotIndex);
        }
    }

    // 룬 정보 반환
    public ItemInstance GetEquippedRune(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= runeSlots.Count) return null;
        return runeSlots[slotIndex]?.RuneItem;
    }

    // 장착된 모든 룬의 능력치 총합 계산
    public EquipItemStats GetTotalEquippedRuneStats()
    {
        return runeSlots
            .Where(slot => slot.RuneItem != null)
            .Aggregate(new EquipItemStats(), (totalStats, slot) =>
            {
                RuneStat stat = slot.RuneItem.RuneStat;
                switch (stat.AbilityType)
                {
                    case AbilityType.ATK: totalStats.ATK += stat.Value; break;
                    case AbilityType.SATK: totalStats.SATK += stat.Value; break;
                    case AbilityType.DEF: totalStats.DEF += stat.Value; break;
                    case AbilityType.SDEF: totalStats.SDEF += stat.Value; break;
                    case AbilityType.SPD: totalStats.SPD += stat.Value; break;
                    case AbilityType.Critical: totalStats.Critical += stat.Value; break;
                    case AbilityType.EVA: totalStats.EVA += stat.Value; break;
                    case AbilityType.FS: totalStats.FS += stat.Value; break;
                }
                return totalStats;
            });
    }
}
