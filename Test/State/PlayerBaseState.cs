using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
    }

    public virtual void Enter()
    {
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {
        Vector3 movementDirection = GetMovementDirection();
        float movementSpeed = GetMovementSpeed();
        stateMachine.playerController._rb.MovePosition(stateMachine.playerController.transform.position + movementDirection * movementSpeed * Time.fixedDeltaTime);
    }

    protected void StartAnimation(string animationHash)
    {
        stateMachine.playerController._anim.SetBool(animationHash, true);
    }

    protected void StopAnimation(string animationHash)
    {
        stateMachine.playerController._anim.SetBool(animationHash, false);
    }

    public virtual void Update()
    {
        Move();
    }

    private void ReadMovementInput()
    {
        // 플레이어콘트롤러의 퍼블릭 플레이어콘트롤을 가져옴.
        stateMachine.MovementInput = stateMachine.playerController._playerControls.Player.Move.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();
        Move(movementDirection);
    }

    private Vector3 GetMovementDirection()
    {
        return new Vector3(stateMachine.MovementInput.x, 0, stateMachine.MovementInput.y).normalized;
    }

    private void Move(Vector3 direction)
    {
        Vector3 scale = stateMachine.playerController._scale;

        if (direction.x != 0 && direction.x < 0)
        {
            stateMachine.playerController._playerSprite.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }

        if (direction.x != 0 && direction.x > 0)
        {
            stateMachine.playerController._playerSprite.transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        }
    }

    private float GetMovementSpeed()
    {
        float moveSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return moveSpeed;
    }

    protected virtual void AddInputActionsCallbacks()
    {
        PlayerControls input = stateMachine.playerController._playerControls;
        input.Player.Move.performed += OnMovementCanceled;
        //input.Player.Jump.started += OnJumpStarted;
        //input.Player.Dash.performed += OnDoubleShift;
        input.Player.Run.started += OnRunStarted;
        input.Player.Attack.started += OnAttack;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerControls input = stateMachine.playerController._playerControls;
        input.Player.Move.performed -= OnMovementCanceled;
        //input.Player.Jump.started -= OnJumpStarted;
        //input.Player.Dash.performed -= OnDoubleShift;
        input.Player.Run.started -= OnRunStarted;
        input.Player.Attack.performed -= OnAttack;
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        if (stateMachine.playerController.terrainDetector.isGround == true)
        {
            stateMachine.playerController._rb.AddForce(Vector3.up * 180, ForceMode.Force);
            stateMachine.ChangeState(stateMachine.jumpState);
        }
    }

    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.runState);
    }


    protected virtual void OnDoubleShift(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.dashState);
    }

    protected virtual void OnAttack(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.attackState);
    }
}
