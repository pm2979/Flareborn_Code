public class TurnCountAboveCondition : ITraitCondition // 현제 턴 횟수 확인
{
    private int threshold; // 턴 임계값

    public TurnCountAboveCondition(int threshold)
    {
        this.threshold = threshold;
    }

    // 특성 발동 조건
    public bool Evaluate(PartyEntities owner)
    {
        return owner.TurnCount >= threshold;
    }
}
