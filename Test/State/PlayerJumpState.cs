using UnityEngine;

public class PlayerJumpState : PlayerGroundedState
{
    public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.playerController.animationData.jumpParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.playerController.animationData.jumpParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.playerController.terrainDetector.isGround == false)
        {
            // 걷는 상태로 전환
            stateMachine.ChangeState(stateMachine.idleState);
            return;
        }
    }
}
