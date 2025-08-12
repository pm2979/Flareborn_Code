using UnityEngine;

public class OverWorldEnemyStateMachine : StateMachine
{
    public OverWorldEnemy overWorldEnemy { get; }
    public float MovementSpeed { get; private set; } = 5f;

    // 스테이트
    public OverWorldEnemyIdleState idleState { get; private set; }
    public OverWorldEnemyWanderState wanderState { get; private set; }
    public OverWorldEnemyChaseState chaseState { get; private set; }
    public OverWorldEnemyPrepareAttackState prepareAttackState { get; private set; }
    public OverWorldEnemyAttackState attackState { get; private set; }

    public OverWorldEnemyStateMachine(OverWorldEnemy overWorldEnemy)
    {
        this.overWorldEnemy = overWorldEnemy;
        MovementSpeed = 5f;

        idleState = new OverWorldEnemyIdleState(this);
        wanderState = new OverWorldEnemyWanderState(this);
        chaseState = new OverWorldEnemyChaseState(this);
        prepareAttackState = new OverWorldEnemyPrepareAttackState(this);
        attackState = new OverWorldEnemyAttackState(this);
    }
}
