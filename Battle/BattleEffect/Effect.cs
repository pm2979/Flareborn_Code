using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections;

// 모든 3D 이펙트의 기본이 되는 추상 클래스
public abstract class Effect : MonoBehaviour
{
    [SerializeField] private string soundName;
    [SerializeField] private int hitCount;
    [SerializeField] private float hitInterval;

    public bool IsPlaying { get; protected set; }
    public SoundManager SoundManager { get; protected set; }

    // 이펙트를 초기화
    public virtual void Awake()
    {
        gameObject.SetActive(false);
        IsPlaying = false;
        SoundManager = SoundManager.Instance;
    }

    // 이펙트를 재생하고 Task를 반환
    public abstract Task PlayEffect(Action onImpact = null);

    // 이펙트 즉시 정지
    public abstract void StopEffect();

    // 사운드 재생 코루틴
    protected void PlaySound()
    {
        if (string.IsNullOrEmpty(soundName)) return;

        StartCoroutine(PlaySoundRoutine());
    }

    private IEnumerator PlaySoundRoutine()
    {
        if (SoundManager == null)
        {
            yield break;
        }

        if (hitCount > 1 && hitInterval > 0)
        {
            for (int i = 0; i < hitCount; i++)
            {
                SoundManager.PlaySFX(soundName);
                yield return new WaitForSeconds(hitInterval);
            }
        }
        else
        {
            SoundManager.PlaySFX(soundName);
        }
    }

    protected virtual void OnDisable()
    {
        // 오브젝트가 비활성화될 때 강제로 정지시켜 리소스를 정리
        StopEffect();
    }
}