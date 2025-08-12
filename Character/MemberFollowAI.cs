using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberFollowAI : MonoBehaviour
{
    public Transform followTarget;  // 따라갈 대상 (플레이어)
    public float speed = 3f;
    public float followDistance = 1.5f; // 일정 거리 이상 멀면 따라감

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Vector3 originalScale;

    private const string IS_WALK_PARAM = "IsWalk";

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        if (followTarget == null) return;

        float distance = Vector3.Distance(transform.position, followTarget.position);

        if (distance > followDistance)
        {
            anim.SetBool(IS_WALK_PARAM, true);

            Vector3 direction = (followTarget.position - transform.position).normalized;
            float step = speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, followTarget.position, step);

            // 좌우 반전
            if (direction.x < 0)
            {
                spriteRenderer.transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            }
            else
            {
                spriteRenderer.transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            }
        }
        else
        {
            anim.SetBool(IS_WALK_PARAM, false);
        }
    }

    public void SetFollowDistance(int distance)
    {
        followDistance = distance;
    }
}