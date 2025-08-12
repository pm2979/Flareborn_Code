using System.Collections.Generic;

public interface IEnemyAI
{
    // 어떤 스킬을 누구에게 사용할지 결정
    AIAction DecideAction(EnemyEntities self, List<BattleEntities> players, List<BattleEntities> allies);
}