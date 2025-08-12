using UnityEngine;
using System;
using System.Threading.Tasks;

// 근거리 공격 이펙트를 처리하는 클래스
public class MeleeEffect : Effect
{
    [Header("근거리 이펙트 설정")]
    [SerializeField] private ParticleSystem mainParticle;
    [SerializeField] private float impactTime = 0.3f; // 이펙트 재생 후 onImpact가 호출될 시간

    public override async Task PlayEffect(Action onImpact = null)
    {
        if (mainParticle == null) return;
        if (IsPlaying) return;

        IsPlaying = true;
        gameObject.SetActive(true);
        PlaySound();

        mainParticle.Play();

        float duration = mainParticle.main.duration;

        // 임팩트 콜백 처리
        if (onImpact != null && impactTime > 0 && impactTime < duration)
        {
            await Task.Delay((int)(impactTime * 1000));
            onImpact.Invoke();
            await Task.Delay((int)((duration - impactTime) * 1000));
        }
        else // 임팩트 시간이 없거나 재생시간보다 길면 끝날 때 호출
        {
            await Task.Delay((int)(duration * 1000));
            onImpact?.Invoke();
        }

        IsPlaying = false;
    }

    public override void StopEffect()
    {
        if (mainParticle != null)
        {
            mainParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        IsPlaying = false;
    }
}