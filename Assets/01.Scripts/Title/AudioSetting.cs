using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        masterSlider.onValueChanged.AddListener((float v)=>SoundManager.Instance.ChangeSound(VOLTYPE.MASTER, v));
        bgmSlider.onValueChanged.AddListener((float v) => SoundManager.Instance.ChangeSound(VOLTYPE.BGM, v));
        sfxSlider.onValueChanged.AddListener((float v) => SoundManager.Instance.ChangeSound(VOLTYPE.SFX, v));

        // 초기 슬라이더 값 설정 (슬라이더 최소 값은 0이 아니라 0.0001로 설정할 수 있음)
        masterSlider.value = 1;
        bgmSlider.value = 1;
        sfxSlider.value = 1;
    }

    public void SetMasterVolume(float value)
    {
        // 값이 0일 때는 가장 낮은 볼륨(-80 dB)으로 설정
        float volume = value == 0 ? -80f : Mathf.Log10(value) * 20;
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetBGMVolume(float value)
    {
        // 값이 0일 때는 가장 낮은 볼륨(-80 dB)으로 설정
        float volume = value == 0 ? -80f : Mathf.Log10(value) * 20;
        audioMixer.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float value)
    {
        // 값이 0일 때는 가장 낮은 볼륨(-80 dB)으로 설정
        float volume = value == 0 ? -80f : Mathf.Log10(value) * 20;
        audioMixer.SetFloat("SFXVolume", volume);
    }
}
