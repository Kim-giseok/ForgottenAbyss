using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VOLTYPE
{
    MASTER,
    BGM,
    SFX
}

[Serializable]
public class Volume
{
    public VOLTYPE type;
    [Range(0, 1)] public float arrange = 1;
}

[Serializable]
public class Volumes
{
    public Volume[] list;
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] Volumes baseVolumes;
    public Dictionary<VOLTYPE, float> volumes = new();

    [SerializeField] string volumeSavePath = "volumes.json";

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

        baseVolumes = DataSave<Volumes>.LoadOrBase(baseVolumes, volumeSavePath);

        foreach (var baseVol in baseVolumes.list)
            volumes[baseVol.type] = baseVol.arrange;

        if (!TryGetComponent<AudioSource>(out bgmSource))
            bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        bgmSource.loop = true;
        PlayBgm();
    }

    private void OnDestroy()
    {
        foreach (var baseVol in baseVolumes.list)
            baseVol.arrange = volumes[baseVol.type];

        DataSave<Volumes>.SaveData(baseVolumes, volumeSavePath);
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
