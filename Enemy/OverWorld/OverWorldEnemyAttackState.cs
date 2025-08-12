using UnityEngine;
using UnityEngine.AI;

public class OverWorldEnemyAttackState : OverWorldEnemyBaseState
{
    private float attackDuration = 1f;
    private float timer = 0f;
    private float attackRange;
    private bool hasAttacked = false;

    public OverWorldEnemyAttackState(OverWorldEnemyStateMachine stateMachine) : base(stateMachine)
    {
        attackRange = stateMachine.overWorldEnemy.attackRange;
    }

    public override void Enter()
    {
        base.Enter();
        timer = 0f;
        hasAttacked = false;

        var agent = stateMachine.overWorldEnemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        SetAttacking(true); // 애니메이션 재생
    }

    public override void Exit()
    {
        base.Exit();

        var agent = stateMachine.overWorldEnemy.GetComponent<NavMeshAgent>();
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = false;
        }
        else

        SetAttacking(false); // 애니메이션 종료
    }

    public override void Update()
    {
        base.Update();
        timer += Time.deltaTime;

        if (!hasAttacked && timer >= attackDuration)
        {
            float distanceToPlayer = Vector3.Distance(
                stateMachine.overWorldEnemy.transform.position,
                stateMachine.overWorldEnemy.target.position
            );

            if (distanceToPlayer <= attackRange)
            {
                EncounterSystem.Instance?.TriggerEncounter(
                    stateMachine.overWorldEnemy.enemyInstance,
                    stateMachine.overWorldEnemy.gameObject
                );
            }

            hasAttacked = true;
            stateMachine.ChangeState(stateMachine.idleState); // 쿨타임 없이 바로 상태 전환
        }
    }
}