using System.Collections;
using UnityEngine;

public class NPC_Patrol : MonoBehaviour
{
    public float speed = 2f;
    public float pauseDuration = 1.5f;
    public Vector3[] patrolPoints;

    private bool isPaused = false;
    private Vector3 target;
    private Vector3 direction;
    private int currentPatrolIndex;

    private Rigidbody rb;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        StartCoroutine(SetPatrolPoint());
    }

    private void Update()
    {
        NPCMoves();
    }

    private void NPCMoves()
    {
        if (isPaused)
        {
            // NPC가 멈춘 상태라면 움직이지 않도록 설정
            rb.linearVelocity = Vector3.zero;
            return;
        }
        // 타겟 포인트를 향해 갈 수 있도록 방향을 설정
        direction = (target - transform.position).normalized;
        rb.linearVelocity = direction * speed;

        // 만약 해당 타겟 포인트에 도달했다면, 다음 타겟 포인트를 설정해준 뒤 이동
        if (Vector3.Distance(transform.position, target) < 0.1f)
            StartCoroutine(SetPatrolPoint());

        SetWalkAnim(direction); // 이동 애니메이션 설정
    }

    private IEnumerator SetPatrolPoint()
    {
        isPaused = true; // 일시 정지 상태로 설정
        anim.Play("Idle");

        yield return new WaitForSeconds(pauseDuration);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        target = patrolPoints[currentPatrolIndex];

        isPaused = false; // 일시 정지 상태 해제
    }

    private void SetWalkAnim(Vector3 direction)
    {
        float x = Mathf.Abs(direction.x);
        float z = Mathf.Abs(direction.z);

        if (direction.x < 0)
        {
            anim.Play("Walk_Left");
        }
        else if (direction.x > 0)
        {
            anim.Play("Walk_Right");
        }
    }
}
