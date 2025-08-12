public class StressAboveCondition : ITraitCondition // 스트레스 확인
{
    private int threshold; // 스트레스 임계값

    public StressAboveCondition(int owner)
    {
        this.threshold = owner;
    }


    // 특성 발동 조건을 평가합니다.
    public bool Evaluate(PartyEntities owner)
    {
        if (owner.Stress != null)
        {
            return owner.Stress.CurrentStress >= threshold;
        }

        return false;
    }
}
