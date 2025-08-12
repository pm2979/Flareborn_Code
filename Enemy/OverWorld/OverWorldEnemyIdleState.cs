using UnityEngine;

public class OverWorldEnemyIdleState : OverWorldEnemyBaseState
{
    private float idleTime;
    private float elapsedTime;

    public OverWorldEnemyIdleState(OverWorldEnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        idleTime = Random.Range(3f, 6f);
        elapsedTime = 0f;
    }

    public override void Update()
    {
        base.Update();
        elapsedTime += Time.deltaTime;

        if (DetectPlayer())
        {
            stateMachine.ChangeState(stateMachine.chaseState);
            return;
        }

        if (elapsedTime >= idleTime)
        {
            stateMachine.ChangeState(stateMachine.wanderState);
        }
    }

    private bool DetectPlayer()
    {
        float detectRange = 5f;
        var player = PlayerManager.Instance;
        if (player == null) return false;

        float dist = Vector3.Distance(stateMachine.overWorldEnemy.transform.position, player.transform.position);
        return dist <= detectRange;
    }
}