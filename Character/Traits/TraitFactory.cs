using System.Collections.Generic;
using static DesignEnums;

public static class TraitFactory
{
    public static TraitConditionsLoader TraitConditionsLoader { get; private set; }
    public static TraitEffectsLoader TraitEffectsLoader { get; private set; }
    public static TraitDataLoader TraitsLoader { get; private set; }

    public static void Init(TraitConditionsLoader traitConditionsLoader, TraitEffectsLoader traitEffectsLoader, TraitDataLoader traitsLoader)
    {
        TraitConditionsLoader = traitConditionsLoader;
        TraitEffectsLoader = traitEffectsLoader;
        TraitsLoader = traitsLoader;
    }

    public static List<ITraitCondition> CreateCondition(List<int> ids) // 발동 조건
    {
        List<ITraitCondition> conditions = new List<ITraitCondition>();

        foreach (int id in ids)
        {
            TraitConditions condition = TraitConditionsLoader.GetByKey(id);

            switch(condition.ConditionType)
            {
                case TraitCondition.StressAbove:
                    conditions.Add(new StressAboveCondition(condition.Parameter));
                    continue;
                case TraitCondition.TurnCountAbove:
                    conditions.Add(new TurnCountAboveCondition(condition.Parameter));
                    continue;
                case TraitCondition.HealthBelowPercentage:
                    conditions.Add(new HealthBelowPercentageCondition(condition.Parameter));
                    continue;
            };
        }

        return conditions;
    }

    public static List<ITraitEffect> CreateEffect(List<int> ids) // 발동 효과
    {
        List<ITraitEffect> effects = new List<ITraitEffect>();

        foreach (int id in ids)
        {
            TraitEffects effect = TraitEffectsLoader.GetByKey(id);

            switch(effect.EffectType)
            {
                case TraitEffect.UnlockEmotion:
                    effects.Add(new UnlockEmotionEffect(effect.EmotionType));
                    continue;
            };
        }

        return effects;
    }

    public static Trait CreatTrait(int Key)
    {
        TraitData traitData = TraitsLoader.GetByKey(Key);

        List<ITraitCondition> conditions = CreateCondition(traitData.ConditionalID);
        List<ITraitEffect> effects = CreateEffect(traitData.EffectID);

        return new Trait(traitData, conditions, effects);
    }
}