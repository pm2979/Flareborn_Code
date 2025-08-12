using UnityEngine;
using UnityEngine.AI;

public class OverWorldEnemyWanderState : OverWorldEnemyBaseState
{
    private Vector3 targetPos;
    private NavMeshAgent agent;

    private enum WanderPhase { Moving, Waiting }
    private WanderPhase currentPhase;

    private float waitTimer = 0f;
    private float waitDuration = 0f;

    public OverWorldEnemyWanderState(OverWorldEnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        agent = stateMachine.overWorldEnemy.GetComponent<NavMeshAgent>();
        agent.speed = stateMachine.MovementSpeed * 0.5f;

        ChooseNewTarget();
        currentPhase = WanderPhase.Moving;
        
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

        if (DetectPlayer())
        {
            SetMoving(false);
            stateMachine.ChangeState(stateMachine.chaseState);
            return;
        }

        switch (currentPhase)
        {
            case WanderPhase.Moving:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    currentPhase = WanderPhase.Waiting;
                    waitDuration = Random.Range(2f, 4f);
                    waitTimer = 0f;
                    SetMoving(false);
                }
                else if (agent.hasPath)
                {
                    Vector3 dir = agent.velocity.normalized;
                    stateMachine.overWorldEnemy.LookAtDirection(dir);
                }
                break;

            case WanderPhase.Waiting:
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitDuration)
                {
                    ChooseNewTarget();
                    currentPhase = WanderPhase.Moving;
                    SetMoving(true);
                }
                break;
        }
    }

    private void ChooseNewTarget()
    {
        Vector3 origin = stateMachine.overWorldEnemy.transform.position;
        float radius = Random.Range(6f, 10f);

        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPoint = origin + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                targetPos = hit.position;
                agent.SetDestination(targetPos);
                return;
            }
        }

        targetPos = origin;
        agent.ResetPath();
    }

    private bool DetectPlayer()
    {
        float detectRange = 5f;
        var player = PlayerManager.Instance.gameObject;
        if (player == null) return false;

        float dist = Vector3.Distance(stateMachine.overWorldEnemy.transform.position, player.transform.position);
        return dist <= detectRange;
    }
}