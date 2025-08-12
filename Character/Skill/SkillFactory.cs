using System;

public static class SkillFactory
{
    private static SkillDataLoader loader;

    public static void Init(SkillDataLoader dataLoader)
    {
        loader = dataLoader;
    }

    public static SkillInstance Create(int skillIndex)
    {
        SkillData data = loader.GetByKey(skillIndex);

        return data.SkillType switch
        {
            DesignEnums.SkillType.Attack => new AttackSkill(data),
            DesignEnums.SkillType.Heal => new HealSkill(data),
            DesignEnums.SkillType.Buff => new BuffSkill(data),
            DesignEnums.SkillType.Debuff => new DeBuffSkill(data),
            DesignEnums.SkillType.Stress => new StressSkill(data),
            _ => throw new NotImplementedException($"스킬 타입 {data.SkillType}은 아직 구현되지 않았습니다.")
        };
    }

    public static SkillInstance Create(SkillData data)
    {
        return data.SkillType switch
        {
            DesignEnums.SkillType.Attack => new AttackSkill(data),
            DesignEnums.SkillType.Heal => new HealSkill(data),
            DesignEnums.SkillType.Buff => new BuffSkill(data),
            DesignEnums.SkillType.Debuff => new DeBuffSkill(data),
            DesignEnums.SkillType.Stress => new StressSkill(data),
            _ => throw new NotImplementedException($"스킬 타입 {data.SkillType}은 아직 구현되지 않았습니다.")
        };
    }
}

