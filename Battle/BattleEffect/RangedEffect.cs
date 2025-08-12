using UnityEngine;
using DG.Tweening;
using System;
using System.Threading.Tasks;

// 원거리 발사체 이펙트를 처리하는 클래스
public class RangedEffect : Effect
{
    [Header("이동 설정")]
    [SerializeField] private GameObject projectileObject; // 발사체 오브젝트
    [SerializeField] private Transform projectilePos; // 발사체 오브젝트
    [SerializeField] private float moveDuration = 1.0f;
    [SerializeField] private Ease moveEase = Ease.Linear;

    [Header("타격 설정")]
    [SerializeField] private ParticleSystem hitEffect;

    private Tween moveTween;

    public override async Task PlayEffect(Action onImpact = null)
    {
        if (IsPlaying) return;

        IsPlaying = true;
        gameObject.SetActive(true);

        projectileObject.transform.position = projectilePos.position;
        projectileObject.SetActive(true);
        if (hitEffect) hitEffect.gameObject.SetActive(false);

        PlaySound();

        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        moveTween?.Kill();
        moveTween = projectileObject.transform.DOMove(transform.position , moveDuration)
            .SetEase(moveEase)
            .OnComplete(async () => {
                projectileObject.SetActive(false); // 발사체 숨김
                onImpact?.Invoke(); // 임팩트 콜백 호출

                if (hitEffect != null)
                {
                    hitEffect.gameObject.SetActive(true);
                    hitEffect.Play();
                    await Task.Delay((int)(hitEffect.main.duration * 1000));
                }

                tcs.SetResult(true);
            });

        await tcs.Task;
        IsPlaying = false;
    }

    public override void StopEffect()
    {
        moveTween?.Kill();
        if (projectileObject) projectileObject.SetActive(true);
        if (hitEffect) hitEffect.gameObject.SetActive(false);
        IsPlaying = false;
    }
}