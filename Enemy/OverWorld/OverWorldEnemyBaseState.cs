using UnityEngine;

public class OverWorldEnemyBaseState : IState
{
    protected OverWorldEnemyStateMachine stateMachine;

    public OverWorldEnemyBaseState(OverWorldEnemyStateMachine overWorldEnemyStateMachine)
    {
        stateMachine = overWorldEnemyStateMachine;
    }

    public virtual void Enter()
    {
        AddInputActionsCallbacks();
        SetAttacking(false); // 기본적으로 공격 아님
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput() { }

    public virtual void PhysicsUpdate() { }

    public virtual void Update() { }

    protected virtual void AddInputActionsCallbacks() { }

    protected virtual void RemoveInputActionsCallbacks() { }

    protected void SetAttacking(bool isAttacking)
    {
        stateMachine.overWorldEnemy._anim.SetBool("IsAttacking", isAttacking);
    }
    
    protected void SetMoving(bool isMoving)
    {
        stateMachine.overWorldEnemy._anim.SetBool("IsMoving", isMoving);
    }
}