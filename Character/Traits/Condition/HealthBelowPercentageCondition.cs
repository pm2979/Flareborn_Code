public class HealthBelowPercentageCondition : ITraitCondition // 체력 비율 확인
{
    private int percentageThreshold; // 체력 비율 임계값

    public HealthBelowPercentageCondition(int threshold)
    {
        this.percentageThreshold = threshold;
    }

    // 특성 발동 조건
    public bool Evaluate(PartyEntities owner)
    {
        if (owner.MaxHP <= 0)
        {
            return false;
        }

        // 현재 체력 비율 계산
        float currentHealthPercentage = ((float)owner.CurrHP / owner.MaxHP) * 100f;

        return currentHealthPercentage <= percentageThreshold;
    }
}
