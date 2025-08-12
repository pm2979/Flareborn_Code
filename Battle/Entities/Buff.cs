using static DesignEnums;

public class Buff // 버프/디버프의 상태를 저장
{
    public BuffType BuffType { get; private set; }
    public float Amount { get; private set; } // 변화량 (+ 또는 -)
    public int Duration { get; set; } // 남은 턴 수

    public BattleEntities Target { get; private set; }

    public bool IsActive { get; private set; }

    public Buff(BuffType buffType, float amount, int duration, BattleEntities target)
    {
        BuffType = buffType;
        Amount = amount;
        Duration = duration;
        Target = target;
        IsActive = true;
    }

    public virtual void OnTurnEnd() { } // 턴 시작 시 호출될 가상 메소드

    public virtual void OnTurnStart() // 지속시간 감소
    {
        if (!IsActive) return;
        if (Duration >= 100) return;

        Duration--;
        if (Duration <= 0)
        {
            Deactivate();
        }
    }

    protected void Deactivate()
    {
        IsActive = false;
    }
}

public class BleedBuff : Buff
{
    public BleedBuff(BuffType buffType, float amount, int duration, BattleEntities target) : base(buffType, amount, duration, target)
    {
    }

    public override void OnTurnEnd()
    {
        OnTurnEndAsync();
    }

    public async void OnTurnEndAsync()
    {
        if (IsActive)
        {
            await Target.DealDamageByyPercent(Amount);
        }
    }
}

public class HealBuff : Buff
{
    public HealBuff(BuffType buffType, float amount, int duration, BattleEntities target) : base(buffType, amount, duration, target)
    {
    }

    public override void OnTurnEnd()
    {
        OnTurnEndAsync();
    }

    public async void OnTurnEndAsync()
    {
        if (IsActive)
        {
            await Target.HealByPercent(Amount);
        }
    }
}
