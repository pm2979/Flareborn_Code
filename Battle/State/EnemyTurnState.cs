using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyTurnState : IBattleState
{
    public void EnterState(BattleSystem system)
    {
        EnemyEntities enemy = system.CurrentEnemyEntity();

        if (!enemy.IsPlayer && enemy.EnemyAI != null)
        {
            system.StartCoroutine(ExecuteEnemyTurn(system, enemy));
        }
        else
        {
            system.StartCoroutine(system.EndCurrentTurn());
        }
    }

    private IEnumerator ExecuteEnemyTurn(BattleSystem system, EnemyEntities enemy)
    {
        // AI 행동 결정 요청
        AIAction action = enemy.EnemyAI.DecideAction(enemy, system.PartyBattlers, system.EnemyBattlers);

        if (action != null && action.Skill != null && action.Targets.Any())
        {
            // 스킬 실행 코루틴이 끝날 때까지 대기
            yield return system.ActionController.PlaySkill(enemy, action.Skill, action.Targets);
        }
        else
        {
            // AI가 행동을 결정하지 못하는 예외 상황
            yield return new WaitForSeconds(1.0f);
        }

        // 턴 종료 코루틴을 실행하고 끝날 때까지 대기
        yield return system.EndCurrentTurn();
    }

    public void ExitState(BattleSystem system)
    {

    }
}
