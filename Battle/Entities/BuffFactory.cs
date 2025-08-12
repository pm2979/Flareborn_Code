using System.Collections.Generic;
using static DesignEnums;

public static class BuffFactory
{
    public static BuffDataLoader BuffDataLoader { get; private set; }

    public static void Init(BuffDataLoader loader)
    {
        BuffDataLoader = loader;
    }


    public static Buff CreateBuff(BuffType buffType, float amount, int duration, BattleEntities target)
    {
        switch (buffType)
        {
            case BuffType.IncreaseDEF:
            case BuffType.DecreaseDEF:
            case BuffType.IncreaseATK:
            case BuffType.DecreaseATK:
            case BuffType.IncreaseSATK:
            case BuffType.DecreaseSATK:
            case BuffType.IncreaseSDEF:
            case BuffType.DecreaseSDEF:
            case BuffType.IncreaseSPD:
            case BuffType.DecreaseSPD:
            case BuffType.IncreaseCritical:
            case BuffType.DecreaseCritical:
            case BuffType.IncreaseEVA:
            case BuffType.DecreaseEVA:
                return new Buff(buffType, amount, duration, target);
            case BuffType.BleedBuff:
                return new BleedBuff(buffType, amount, duration, target);
            case BuffType.HealBuff:
                return new HealBuff(buffType, amount, duration, target);
            default:
                return null;
        }
    }

    public static List<Buff> CreateBuffs(BuffName buffName, BattleEntities target)
    {
        BuffData buffData = BuffDataLoader.GetByKey((int)buffName);

        List<Buff> buffList = new List<Buff>();

        for(int i = 0; i < buffData.BuffType.Count; i++ )
        {
            Buff buff = CreateBuff(buffData.BuffType[i], buffData.BuffPower[i], buffData.Duration, target);
            buffList.Add(buff);
        }

        return buffList;
    }
}
