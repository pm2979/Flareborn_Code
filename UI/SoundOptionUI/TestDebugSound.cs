using System.Collections;
using UnityEngine;

public class TestDebugSound : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(PlayBGMWhenReady());
    }

    private IEnumerator PlayBGMWhenReady()
    {
        // SoundManager와 SoundSettings가 초기화될 때까지 대기
        while (SoundManager.Instance == null || !SoundSettings.IsInitialized)
        {
            yield return null;
        }

        // 볼륨 설정이 확실히 적용되도록 한 번 더 적용
        SoundSettings.ApplyVolumesToSoundManager();

        // 추가 대기 (안전장치)
        yield return new WaitForEndOfFrame();

        Debug.Log("[TestDebugSound] 볼륨 설정 완료, BGM 재생 시작");

        // 이제 안전하게 BGM 재생
        SoundManager.Instance.PlayBGM("Test");
    }
}