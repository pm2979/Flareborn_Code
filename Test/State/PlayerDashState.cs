using UnityEngine;

public class PlayerDashState : PlayerGroundedState
{
    private float dashEndTime;

    public PlayerDashState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartDashWithForce();
        StartAnimation(stateMachine.playerController.animationData.dashParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.playerController.animationData.dashParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (Time.time >= dashEndTime)
        {
            stateMachine.ChangeState(stateMachine.idleState);
        }
    }

    void StartDashWithForce()
    {
        dashEndTime = Time.time + stateMachine.dashDuration;
        if (stateMachine.MovementInput.y == 0) // W와 S키가 Y좌표라 transform 방향이 위,아래로 됨.
        {
            stateMachine.playerController._rb.AddForce(stateMachine.MovementInput * stateMachine.dashForce, ForceMode.Impulse);
        }
        else // 앞 뒤로 바꾸어줌.
        {
            stateMachine.playerController._rb.AddForce(stateMachine.playerController.transform.forward * stateMachine.dashForce, ForceMode.Impulse);
        }
    }

}
