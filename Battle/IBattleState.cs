public interface IBattleState
{
    void EnterState(BattleSystem system); // 상태 진입
    void ExitState(BattleSystem system); // 상태 탈출
}
