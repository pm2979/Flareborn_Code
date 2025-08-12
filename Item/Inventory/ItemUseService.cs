using System.Collections.Generic;
using UnityEngine;
using static DesignEnums;

public static class ItemUseService
{
    // 소비 아이템 효과를 대상에게 적용
    public static void ApplyConsumableEffect(ItemInstance item, List<CharacterInstance> targets)
    {
        if (!IsValid(item, targets)) return;

        var effect = item.ConsumableEffect;

        // 수량이 부족하면 효과 적용하지 않음
        if (item.CurrentStack < targets.Count)
        {
            Debug.LogWarning($"[ItemUseService] 아이템 수량 부족: 보유 {item.CurrentStack}, 필요 {targets.Count}");
            return;
        }

        // 대상마다 효과 적용
        foreach (var target in targets)
        {
            ApplyEffectToTarget(effect, target);
        }

        // 사용한 만큼 수량 차감
        item.RemoveStack(targets.Count);
    }

    // 유효성 검사
    private static bool IsValid(ItemInstance item, List<CharacterInstance> targets)
    {
        if (item == null || item.ConsumableEffect == null)
        {
            Debug.LogWarning("[ItemUseService] 아이템 또는 효과 정보가 null입니다.");
            return false;
        }

        if (targets == null || targets.Count == 0)
        {
            Debug.LogWarning("[ItemUseService] 대상이 없습니다.");
            return false;
        }

        return true;
    }

    // 효과 종류에 따라 분기 처리
    private static void ApplyEffectToTarget(ConsumableEffects effect, CharacterInstance target)
    {
        switch (effect.ConsumableType)
        {
            case ConsumableType.HealHP:
                ApplyHeal(target, effect.Amount);
                break;

            default:
                Debug.LogWarning($"[ItemUseService] 처리되지 않은 효과 타입: {effect.ConsumableType}");
                break;
        }
    }

    // 체력 회복 적용
    private static void ApplyHeal(CharacterInstance target, int amount)
    {
        int newHp = Mathf.Min(target.CurrentHp + amount, target.MaxHp);
        target.SetCurrentHp(newHp);
        Debug.Log($"[Heal] {target.Name} → HP +{amount} → {target.CurrentHp}/{target.MaxHp}");
    }
}
