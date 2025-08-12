using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public PlayerController playerController { get; }
    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; } = 3f;
    public float MovementSpeedModifier { get; set; } = 1f;
    public float dashForce { get; set; } = 5f; // 대시 속도
    public float dashDuration { get; set; } = 0.2f; // 대시 지속 시간

    // 스테이트
    public PlayerIdleState idleState { get; private set; }
    public PlayerWalkState walkState { get; private set; }
    public PlayerRunState runState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerAttackState attackState { get; private set; }

    public PlayerStateMachine(PlayerController playerController)
    {
        this.playerController = playerController;
        MovementSpeed = 3f;
        idleState = new PlayerIdleState(this);
        walkState = new PlayerWalkState(this);
        runState = new PlayerRunState(this);
        jumpState = new PlayerJumpState(this);
        dashState = new PlayerDashState(this);
        attackState = new PlayerAttackState(this);
    }


}
