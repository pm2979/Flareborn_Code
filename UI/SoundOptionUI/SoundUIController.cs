using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundUIController : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("Volume Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        // 초기값 불러오기
        masterSlider.value = LoadVolume("MasterVolume");
        bgmSlider.value = LoadVolume("BGMVolume");
        sfxSlider.value = LoadVolume("SFXVolume");

        // 초기 볼륨 적용
        SetMaster(masterSlider.value);
        SetBGM(bgmSlider.value);
        SetSFX(sfxSlider.value);

        // 슬라이더 이벤트 연결
        masterSlider.onValueChanged.AddListener(SetMaster);
        bgmSlider.onValueChanged.AddListener(SetBGM);
        sfxSlider.onValueChanged.AddListener(SetSFX);
        
    }

    private void SetMaster(float value)
    {
        value = Mathf.Max(value, 0.0001f);
        mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20f);
        SoundSettings.SaveVolume("MasterVolume", value);
    }

    private void SetBGM(float value)
    {
        value = Mathf.Max(value, 0.0001f);
        mixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20f);
        SoundSettings.SaveVolume("BGMVolume", value);
    }

    private void SetSFX(float value)
    {
        value = Mathf.Max(value, 0.0001f);
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20f);
        SoundSettings.SaveVolume("SFXVolume", value);
    }

    private float LoadVolume(string key, float defaultValue = 1f)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }
    
}

