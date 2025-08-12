using UnityEngine;
using UnityEngine.AI;

public class OverWorldEnemyPrepareAttackState : OverWorldEnemyBaseState
{
    private float prepareDuration = 0.5f;
    private float timer = 0f;
    private float attackRange;

    public OverWorldEnemyPrepareAttackState(OverWorldEnemyStateMachine stateMachine) : base(stateMachine)
    {
        attackRange = stateMachine.overWorldEnemy.attackRange;
    }

    public override void Enter()
    {
        base.Enter();
        timer = 0f;

        var agent = stateMachine.overWorldEnemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }

    public override void Update()
    {
        base.Update();
        timer += Time.deltaTime;

        if (timer < prepareDuration) return;

        float distanceToPlayer = Vector3.Distance(
            stateMachine.overWorldEnemy.transform.position,
            stateMachine.overWorldEnemy.target.position
        );

        if (distanceToPlayer > attackRange)
        {
            stateMachine.ChangeState(stateMachine.chaseState);
            return;
        }

        stateMachine.ChangeState(stateMachine.attackState);
    }
}