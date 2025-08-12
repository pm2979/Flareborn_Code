using UnityEngine;

public class PlayerRunState : PlayerGroundedState
{
    public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 3f;
        base.Enter();
        StartAnimation(stateMachine.playerController.animationData.walkParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.playerController.animationData.walkParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.MovementInput == Vector2.zero)
        {
            // 일반 상태로 전환
            stateMachine.ChangeState(stateMachine.idleState);
            return;
        }
    }
}
