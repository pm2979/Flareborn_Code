using System;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    // 연결된 컴포넌트
    private Animator animator;

    private EnemyBattleVisuals enemyBattle;


    // 애니메이션 트리거 이름 상수값
    private const string IS_ATTACK_PARAM = "IsAttack";
    private const string IS_HIT_PARAM = "IsHit";
    private const string IS_DEAD_PARAM = "IsDead";
    private const string IS_Walk_PARAM = "IsWalk";
    
    private Action onAnimationComplete;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (GetComponentInParent<EnemyBattleVisuals>() != null)
            enemyBattle = gameObject.GetComponentInParent<EnemyBattleVisuals>();
    }

    public void PlayAttackAnimation(Action onComplete = null)
    {
        animator.SetTrigger(IS_ATTACK_PARAM);
        this.onAnimationComplete = onComplete;
    }

    public void PlayAnimation(string action,Action onComplete = null)
    {
        animator.SetTrigger(action);
        this.onAnimationComplete = onComplete;
    }

    public void OnAttackAnimationEnd() // 애니매이션 끝났는지 확인
    {
        onAnimationComplete?.Invoke();
    }

    public void PlayHitAnimation()
    {
        animator.SetTrigger(IS_HIT_PARAM);
    }

    public void PlayDeathAnimation()
    {
        if (animator == null || !animator.isActiveAndEnabled) return;

        animator.SetTrigger(IS_DEAD_PARAM);
    }

    public void PlayWalkAnimation(bool isWalk)
    {
        animator.SetBool(IS_Walk_PARAM, isWalk);
    }

    public void DestroyAction()
    {
        if (enemyBattle != null)
        {
            Destroy(enemyBattle.gameObject);
        }
    }
}
