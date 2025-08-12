public class BattleStartState : IBattleState
{
    public void EnterState(BattleSystem system)
    {
        system.StartTurnLoading();
    }

    public void ExitState(BattleSystem system)
    {
        // 별도 종료 처리 없음
    }
}