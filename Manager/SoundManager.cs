using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name; // 사운드 이름
    public AudioClip clip; // 오디오 클립
    public float defaultVolume = 1.0f; // 기본 볼륨
    public float defaultPitch = 1.0f; // 기본 음높이
}

public class SoundManager : MonoSingleton<SoundManager>
{
    [Header("사운드 등록")]
    [SerializeField] private List<Sound> bgmClips;
    [SerializeField] private List<Sound> sfxClips;

    [Header("오디오 소스 설정")]
    [SerializeField] private AudioSource bgmPlayer;

    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerGroup masterMixerGroup;
    [SerializeField] private AudioMixerGroup bgmMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    [Header("SFX 풀링 설정")]
    [SerializeField] private int sfxPoolSize = 10;
    private Queue<AudioSource> sfxPool;

    // 빠른 조회를 위한 사운드 딕셔너리
    private Dictionary<string, Sound> bgmDictionary;
    private Dictionary<string, Sound> sfxDictionary;

    // AudioMixer 접근을 위한 public 프로퍼티
    public AudioMixer AudioMixer => mixer;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        Init();
    }

    private void Start()
    {
        PlayBGMStart("Start");
    }

    // 초기화
    private void Init()
    {
        bgmDictionary = CreateSoundDictionary(bgmClips);
        sfxDictionary = CreateSoundDictionary(sfxClips);
        InitializeSFXPool();

        if (bgmPlayer != null && bgmMixerGroup != null)
        {
            bgmPlayer.outputAudioMixerGroup = bgmMixerGroup;
        }
    }

    // 딕셔너로 변환
    private Dictionary<string, Sound> CreateSoundDictionary(List<Sound> soundList)
    {
        var dict = new Dictionary<string, Sound>();
        foreach (Sound sound in soundList)
        {
            if (!string.IsNullOrEmpty(sound.name) && sound.clip != null)
            {
                dict[sound.name] = sound;
            }
        }
        return dict;
    }

    // SFX 오디오 소스 생성 및 풀에 저장
    private void InitializeSFXPool()
    {
        sfxPool = new Queue<AudioSource>();
        GameObject sfxPoolContainer = new GameObject("SFX_Pool_Container");
        sfxPoolContainer.transform.SetParent(transform);

        for (int i = 0; i < sfxPoolSize; i++)
        {
            sfxPool.Enqueue(CreateSFXPlayer(sfxPoolContainer.transform, i));
        }
    }

    // SFX 재생 게임 오브젝트를 생성
    private AudioSource CreateSFXPlayer(Transform parent, int index)
    {
        GameObject sfxPlayerObject = new GameObject($"SFXPlayer_{index}");
        sfxPlayerObject.transform.SetParent(parent);
        AudioSource sfxPlayer = sfxPlayerObject.AddComponent<AudioSource>();
        sfxPlayer.playOnAwake = false;
        sfxPlayer.outputAudioMixerGroup = sfxMixerGroup;
        return sfxPlayer;
    }

    // BGM 재생
    public void PlayBGM(string name, float volume = 1.0f)
    {
        if (!bgmPlayer)
        {
            Debug.LogError("BGM Player가 설정되지 않았습니다.");
            return;
        }

        if (bgmDictionary.TryGetValue(name, out Sound sound))
        {
            bgmPlayer.clip = sound.clip;
            bgmPlayer.volume = volume * sound.defaultVolume;
            bgmPlayer.loop = true;
            bgmPlayer.Play();
        }
        else
        {
            Debug.LogWarning($"BGM을 찾을 수 없습니다: {name}");
        }
    }

    // BGM 정지
    public void StopBGM()
    {
        bgmPlayer?.Stop();
    }

    // SFX 재생
    public void PlaySFX(string name, float volume = 1.0f, float pitch = 1.0f)
    {
        if (sfxDictionary.TryGetValue(name, out Sound sound))
        {
            AudioSource sfxPlayer = GetSFXPlayer();
            sfxPlayer.clip = sound.clip;
            sfxPlayer.volume = volume * sound.defaultVolume;
            sfxPlayer.pitch = pitch * sound.defaultPitch;
            sfxPlayer.Play();
            StartCoroutine(ReturnToPoolAfterPlay(sfxPlayer));
        }
        else
        {
            Debug.LogWarning($"SFX를 찾을 수 없습니다: {name}");
        }
    }

    private AudioSource GetSFXPlayer()
    {
        if (sfxPool.Count > 0)
        {
            return sfxPool.Dequeue();
        }
        else
        {
            // 풀이 비었을 때 동적으로 생성
            Transform container = transform.Find("SFX_Pool_Container");
            return CreateSFXPlayer(container, sfxPool.Count); // 임시 인덱스 사용
        }
    }

    // SFX 오디오 소스 반환
    private IEnumerator ReturnToPoolAfterPlay(AudioSource source)
    {
        // isPlaying이 false가 될 때까지, 즉 실제 재생이 끝날 때까지 대기
        yield return new WaitWhile(() => source.isPlaying);
        sfxPool.Enqueue(source);
    }

    private void PlayBGMStart(string name)
    {
        SoundSettings.ApplyAllVolumes(mixer); // 볼륨 적용(방어코드)

        float mixerDb;
        mixer.GetFloat("BGMVolume", out mixerDb);
        Debug.Log("[딜레이 후 PlayBGM] 현재 Mixer BGMVolume dB: " + mixerDb);

        PlayBGM(name);
    }

}