using System;
using UnityEngine;
using UnityEngine.AI;

public class OverWorldEnemy : MonoBehaviour
{
    // 자유 움직임 세팅 - 오리지널 위치 기준으로 X,Z 범위 내 랜덤 이동 가능
    private Vector3 originPos;

    [Range(1f, 5f)]
    public float behaviorX = 3f;

    [Range(1f, 5f)]
    public float behaviorZ = 3f;

    // 에너미 조정 값 (Animator와 SpriteRenderer는 필수 컴포넌트일 경우 Awake에서 자동 할당)
    public Animator _anim;
    public SpriteRenderer _sprite;

    // 스테이트 머신
    private OverWorldEnemyStateMachine stateMachine;
    
    // 공격 사기러
    public float attackRange = 2f;
    
    // 타겟(플레이어)
    public Transform target;

    // 데이터 연동
    public EnemyInstance enemyInstance;
    private void Awake()
    {
        originPos = transform.position;

        // Animator, SpriteRenderer 자동 할당 보완
        if (_anim == null)
            _anim = GetComponentInChildren<Animator>();

        if (_sprite == null)
            _sprite = GetComponentInChildren<SpriteRenderer>();

        stateMachine = new OverWorldEnemyStateMachine(this);
        
        var agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.updateRotation = false;
        }
    }

    private void Start()
    {
        var playerObj = PlayerManager.Instance.gameObject;
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
    }

    private void Update()
    {
        stateMachine.Update(); // 스테이트 머신 업데이트
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate(); // 스테이트 머신 피직스 업데이트
    }

    public void Init(EnemyInstance instance)
    {
        enemyInstance = instance;

        if (stateMachine == null)
            stateMachine = new OverWorldEnemyStateMachine(this);

        stateMachine.ChangeState(stateMachine.idleState);
    }
    
    public void LookAtDirection(Vector3 direction)
    {
        if (direction.x < -0.01f)
            _sprite.flipX = true;
        else if (direction.x > 0.01f)
            _sprite.flipX = false;
    }
}