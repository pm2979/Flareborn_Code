using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
public enum MovementState { Idle, Walk, Jump, Attack }
public enum MovementDirection { Front, Back, Left, Right }

public class PlayerMovement : MonoBehaviour
{
    [Header("Connected Components")]
    [SerializeField] private Animator animator;
    private DialogueEvents dialogueEvents;

    [Header("Movement Settings")]
    [SerializeField] private float currentMoveSpeed;
    [SerializeField] private float walkSpeed = 3f; // 걷기 속도
    [SerializeField] private float runSpeed = 5f; // 달리기 속도

    [Header("Attack Settings")]
    private bool isAttacking = false;
    private float attackDuration = 0.55f; // 공격 쿨타임
    private Coroutine attackCoroutine;
    private WaitForSeconds attackWait;

    private MovementState currentState;
    private MovementDirection currentDirection;
    
    private MovementState lastState;
    private MovementDirection lastDirection;
    
    private Vector2 moveInput;

    private bool isInDialogue = false; // 대화중인지 여부

    // 프로퍼티
    public bool IsAttacking { get { return isAttacking; } set { isAttacking = value; } }
    public bool IsInDialogue { get { return isInDialogue; } set { isInDialogue = value; } }
    public MovementState CurrentState { get { return currentState; } set { currentState = value; } }
    public MovementDirection CurrentDirection { get { return currentDirection; } set { currentDirection = value; } }

    //--------------------------------------------
    private void OnEnable()
    {
        dialogueEvents = GameEventsManager.Instance.dialogueEvents;

        dialogueEvents.onDialogueStarted += DialogueStarted;
        dialogueEvents.onDialogueFinished += DialogueFinished;
    }

    private void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        // Attack 애니메이션 waitforseconds 캐싱
        if (attackWait == null)
            attackWait = new WaitForSeconds(attackDuration);

        // 기본 걷기 속도 설정
        currentMoveSpeed = walkSpeed;
    }

    private void Update()
    {
        // 공격중에는 이동 인풋 받지 않음
        if (isAttacking) return;

        // 대화중에는 이동 불가
        if (isInDialogue) return;

        // 플레이어의 이동 상태를 업데이트
        PerformMove();

        // 플레이어의 애니메이션을 재생
        if (currentState != lastState || currentDirection != lastDirection)
        {
            PlayAnimation(currentState, currentDirection);
            lastState = currentState;
            lastDirection = currentDirection;
        }
    }

    private void OnDisable()
    {
        // # 방어로직
        if (dialogueEvents == null) return;

        dialogueEvents.onDialogueStarted -= DialogueStarted;
        dialogueEvents.onDialogueFinished -= DialogueFinished;
        
        // 씬이 꺼질 때 애니메이션 Attack 프레임이면 Idle로 변경
        if (animator != null)
        {
            string animName = $"Idle_{currentDirection}";
            animator.Play(animName);
        }
    }

    //--------------------------------------------
    public void OnMove(InputAction.CallbackContext context)
    {
        // moveInput을 여기서 읽어와서 저장해준다
        moveInput = context.ReadValue<Vector2>();

        // 인풋 값이 있을 때 방향을 설정
        if (context.performed)
            currentState = MovementState.Walk;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking)
        {
            // # 공격 상태 시작
            isAttacking = true;

            currentState = MovementState.Attack;
            OnAttackState();

            // 공격중에는 moveInput을 초기화
            moveInput = Vector2.zero;
        }    
    }
    
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // 달리기 시작
            currentMoveSpeed = runSpeed;
        }
        else if (context.canceled)
        {
            // 달리기 중지
            currentMoveSpeed = walkSpeed;
        }
    }

    public void DialogueStarted()
    {
        // 대화 중에는 이동 인풋을 초기화
        moveInput = Vector2.zero;
        // 대화 중에는 Idle 상태로 설정
        PlayAnimation(MovementState.Idle, currentDirection);

        // 대화 시작 시 이동 불가
        isInDialogue = true;
    }

    public void DialogueFinished()
    {
        // 대화 종료 시 이동 가능
        isInDialogue = false;
    }

    //--------------------------------------------

    private void PerformMove()
    {
        // 더이상 인풋 값이 없을시 : MovementState.Idle
        if (moveInput == Vector2.zero)
        {
            currentState = MovementState.Idle;
        }

        // 인풋값이 있을 시 : MovementState.Walk
        else if (moveInput != Vector2.zero)
        {
            Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
            transform.position += move * currentMoveSpeed * Time.deltaTime;
            currentState = MovementState.Walk;

            // 현재 방향 설정
            SetDirection();
        }
    }

    private void SetDirection()
    {
        // 오른쪽 왼쪽 이동
        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
        {
            // 오른쪽 왼쪽 방향
            if (moveInput.x > 0)
            {
                currentDirection = MovementDirection.Right;
            }
            else
            {
                currentDirection = MovementDirection.Left;
            }
        }
        // 앞쪽 뒤쪽 방향
        else
        {
            if (moveInput.y > 0)
            {
                currentDirection = MovementDirection.Back;
            }
            else
            {
                currentDirection = MovementDirection.Front;
            }
        }
    }

    //--------------------------------------------

    public void PlayAnimation(MovementState currentMovementState, MovementDirection currentMovementDirection)
    {
        // Update()에서 상시 체크해야하는 애니메이션들
        switch (currentMovementState)
        {
            case MovementState.Idle:
                OnIdleState(currentMovementDirection);
                break;

            case MovementState.Walk:
                OnWalkState(currentMovementDirection);
                break;
            
            case MovementState.Attack:
                OnIdleState(currentMovementDirection);
                break;
        }
    }

    private void OnIdleState(MovementDirection currentMovementDirection)
    {
        switch (currentMovementDirection)
        {
            case MovementDirection.Front:
                animator.CrossFade("Idle_Front", 0);
                break;

            case MovementDirection.Back:
                animator.CrossFade("Idle_Back", 0);
                break;

            case MovementDirection.Left:
                animator.CrossFade("Idle_Left", 0);
                break;

            case MovementDirection.Right:
                animator.CrossFade("Idle_Right", 0);
                break;
        }
    }

    private void OnWalkState(MovementDirection currentMovementDirection)
    {
        switch (currentMovementDirection)
        {
            case MovementDirection.Front:
                animator.CrossFade("Walk_Front", 0);
                break;

            case MovementDirection.Back:
                animator.CrossFade("Walk_Back", 0);
                break;

            case MovementDirection.Left:
                animator.CrossFade("Walk_Left", 0);
                break;

            case MovementDirection.Right:
                animator.CrossFade("Walk_Right", 0);
                break;
        }
    }

    // Attack 버튼 눌렀을때만 체크하면 되는 애니메이션
    private void OnAttackState()
    {
        switch (currentDirection)
        {
            case MovementDirection.Front:
                StartAttackAnimation(MovementDirection.Front);
                break;
            case MovementDirection.Back:
                StartAttackAnimation(MovementDirection.Back);
                break;
            case MovementDirection.Left:
                StartAttackAnimation(MovementDirection.Left);
                break;
            case MovementDirection.Right:
                StartAttackAnimation(MovementDirection.Right);
                break;
        }
    }

    private void StartAttackAnimation(MovementDirection direction)
    {
        // 공격 애니메이션 코루틴 시작
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(AttackAnimationCoroutine(direction));
    }

    private IEnumerator AttackAnimationCoroutine(MovementDirection direction)
    {
        animator.CrossFade($"Attack_{direction.ToString()}", 0);
        yield return attackWait;

        // # 공격 상태 종료
        isAttacking = false;
        
        // 상태 강제 리셋
        currentState = MovementState.Idle;
        PlayAnimation(currentState, currentDirection);
    }
}
