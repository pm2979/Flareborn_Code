using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections;

public static class SoundSettings
{
    private static bool _isInitialized = false;

    // 초기화 (게임 시작 시 호출)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        if (!_isInitialized)
        {
            //SceneManager.sceneLoaded += OnSceneLoaded;
            _isInitialized = true;
            Debug.Log("[SoundSettings] 초기화 완료");
        }
        SetDefaultVolumesIfNotExist();
    }

    // PlayerPrefs에 볼륨 설정이 없는 경우 기본값 (0.5f)으로 설정
    private static void SetDefaultVolumesIfNotExist()
    {
        if (!PlayerPrefs.HasKey("MasterVolume"))
        {
            SaveVolume("MasterVolume", 0.5f);
        }
        if (!PlayerPrefs.HasKey("BGMVolume"))
        {
            SaveVolume("BGMVolume", 0.5f);
        }
        if (!PlayerPrefs.HasKey("SFXVolume"))
        {
            SaveVolume("SFXVolume", 0.5f);
        }
        Debug.Log("[SoundSettings] PlayerPrefs 기본 볼륨 설정 확인 및 적용 완료.");
    }

    // 지정된 키에 볼륨 값을 저장 (PlayerPrefs 사용)
    public static void SaveVolume(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save(); // 즉시 저장
    }

    // 지정된 키로부터 볼륨 값을 불러온다 (없으면 기본값 사용)
    public static float LoadVolume(string key, float defaultValue = 1f)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }

    // AudioMixer에 모든 볼륨 설정 적용
    public static void ApplyAllVolumes(AudioMixer mixer)
    {
        if (mixer == null)
        {
            Debug.LogWarning("[SoundSettings] AudioMixer가 null입니다.");
            return;
        }

        float master = LoadVolume("MasterVolume", 1f);
        float bgm = LoadVolume("BGMVolume", 1f);
        float sfx = LoadVolume("SFXVolume", 1f);

        Debug.Log($"[SoundSettings] 볼륨 적용 - Master: {master}, BGM: {bgm}, SFX: {sfx}");

        // 데시벨 변환하여 적용
        bool masterResult = mixer.SetFloat("MasterVolume", ConvertToDecibels(master));
        bool bgmResult = mixer.SetFloat("BGMVolume", ConvertToDecibels(bgm));
        bool sfxResult = mixer.SetFloat("SFXVolume", ConvertToDecibels(sfx));

        Debug.Log($"[SoundSettings] 볼륨 적용 결과 - Master: {masterResult}, BGM: {bgmResult}, SFX: {sfxResult}");
    }

    // SoundManager를 통해 볼륨을 적용하는 편의 메서드
    public static void ApplyVolumesToSoundManager()
    {
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager != null && soundManager.AudioMixer != null)
        {
            ApplyAllVolumes(soundManager.AudioMixer);
        }
        else
        {
            Debug.LogWarning("[SoundSettings] SoundManager 인스턴스나 AudioMixer를 찾을 수 없습니다.");
        }
    }

    // 볼륨 값을 데시벨로 변환
    private static float ConvertToDecibels(float volume)
    {
        return Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;
    }

    // 초기화 상태 확인
    public static bool IsInitialized => _isInitialized;
}


//// 씬이 로드될 때마다 볼륨 설정 적용
//private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//{
//    // SoundManager가 있다면 볼륨 설정 적용
//    if (SoundManager.Instance != null)
//    {
//        SoundManager.Instance.StartCoroutine(ApplyVolumeAfterDelay());
//    }
//}

//private static IEnumerator ApplyVolumeAfterDelay()
//{
//    // 씬 초기화가 완료될 때까지 잠시 대기
//    yield return new WaitForEndOfFrame();
//    ApplyVolumesToSoundManager();
//}

//// 특정 볼륨만 적용하는 메서드들
//public static void ApplyMasterVolume(AudioMixer mixer)
//{
//    if (mixer != null)
//    {
//        float volume = LoadVolume("MasterVolume", 1f);
//        mixer.SetFloat("MasterVolume", ConvertToDecibels(volume));
//    }
//}

//public static void ApplyBGMVolume(AudioMixer mixer)
//{
//    if (mixer != null)
//    {
//        float volume = LoadVolume("BGMVolume", 1f);
//        mixer.SetFloat("BGMVolume", ConvertToDecibels(volume));
//    }
//}

//public static void ApplySFXVolume(AudioMixer mixer)
//{
//    if (mixer != null)
//    {
//        float volume = LoadVolume("SFXVolume", 1f);
//        mixer.SetFloat("SFXVolume", ConvertToDecibels(volume));
//    }
//}