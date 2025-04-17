using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VOLTYPE
{
    MASTER,
    BGM,
    SFX
}

public class SoundManager : Singleton<SoundManager>
{
    public Dictionary<VOLTYPE, float> volumes = new();

    AudioSource bgmSource, sfxSource;
    [SerializeField] AudioClip bgm;

    [SerializeField] AudioClip[] sfxList;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
        DontDestroyOnLoad(gameObject);

        if (!TryGetComponent<AudioSource>(out bgmSource))
            bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        bgmSource.loop = true;
        PlayBgm();
    }

    public void PlayBgm(AudioClip bgm = null)
    {
        bgmSource.Stop();
        if (bgm != null)
            this.bgm = bgm;
        bgmSource.clip = this.bgm;

        bgmSource.Play();
    }

    public void PlaySFX(AudioClip sfx)
    {
        sfxSource.PlayOneShot(sfx, volumes[VOLTYPE.MASTER] * volumes[VOLTYPE.SFX]);
    }

    public void Playsfx(string sfxName)
    {
        AudioClip sfx = null;
        foreach (var clip in sfxList)
            if (clip.name == sfxName)
            {
                sfx = clip;
                break;
            }

        if (sfx != null)
            PlaySFX(sfx);
    }

    public void ChangeSound(VOLTYPE voltype, float amount)
    {
        volumes[voltype] = amount;
        bgmSource.volume = volumes[VOLTYPE.MASTER] * volumes[VOLTYPE.BGM];
    }
}