using UnityEngine;
using UnityEngine.AI;

public class OverWorldEnemyChaseState : OverWorldEnemyBaseState
{
    private NavMeshAgent agent;
    private Transform playerTransform;
    private float attackRange = 2f;
    private float lostSightRange = 10f;

    public OverWorldEnemyChaseState(OverWorldEnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        agent = stateMachine.overWorldEnemy.GetComponent<NavMeshAgent>();
        agent.speed = stateMachine.MovementSpeed;

        var playerObj = PlayerManager.Instance?.gameObject;
        if (playerObj != null)
            playerTransform = playerObj.transform;
        
        SetMoving(true);
    }

    public override void Exit()
    {
        base.Exit();
        SetMoving(false);
        agent.ResetPath();
    }

    public override void Update()
    {
        base.Update();

        if (playerTransform == null)
        {
            stateMachine.ChangeState(stateMachine.idleState);
            return;
        }

        float dist = Vector3.Distance(stateMachine.overWorldEnemy.transform.position, playerTransform.position);

        if (dist > lostSightRange)
        {
            stateMachine.ChangeState(stateMachine.idleState);
            return;
        }

        agent.SetDestination(playerTransform.position);

        Vector3 dir = (playerTransform.position - stateMachine.overWorldEnemy.transform.position).normalized;
        stateMachine.overWorldEnemy.LookAtDirection(dir);

        if (dist <= attackRange)
        {
            stateMachine.ChangeState(stateMachine.prepareAttackState);
        }
    }
}