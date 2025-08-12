using UnityEngine;

public class PlayerAttackState : PlayerGroundedState
{
    public PlayerAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.playerController._anim.Rebind();
        StartAnimation(stateMachine.playerController.animationData.attackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.playerController.animationData.attackParameterHash);
    }

    public override void Update()
    {
        base.Update();
        AnimatorStateInfo state = stateMachine.playerController._anim.GetCurrentAnimatorStateInfo(0);
        if (state.normalizedTime >= 0.7f)
        {
            stateMachine.ChangeState(stateMachine.idleState);
        }
    }
}
