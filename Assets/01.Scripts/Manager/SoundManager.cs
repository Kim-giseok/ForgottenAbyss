using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

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

public class SoundManager : SingletonLoadRemain<SoundManager>
{
    [SerializeField] Volumes baseVolumes;
    public Dictionary<VOLTYPE, float> volumes = new();

    [SerializeField] string volumeSavePath = "volumes.json";

    public AudioSource bgmSource;
    AudioSource sfxSource;
    [SerializeField] AudioClip bgm;
    private Dictionary<string, AudioClip> addressBGMList = new();


    [SerializeField] AudioClip[] sfxList;
    private Dictionary<string, AudioClip> addressSfxList = new();

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Stage1") { PlayBGM("Combat1"); }
    }

    protected override void Init()
    {
        Addressables.LoadAssetsAsync<AudioClip>("BGM", null).Completed += (handle) =>
        {
            foreach (var clip in handle.Result)
            {
                addressBGMList.Add(clip.name, clip);
            }
        };
        
        Addressables.LoadAssetsAsync<AudioClip>("SFX", null).Completed += (handle) =>
        {
            foreach (var clip in handle.Result)
            {
                addressSfxList.Add(clip.name, clip);
            }
        };
    }

    protected override void Awake()
    {
        base.Awake();

        baseVolumes = DataSave<Volumes>.LoadOrBase(baseVolumes, volumeSavePath);

        foreach (var baseVol in baseVolumes.list)
            volumes[baseVol.type] = baseVol.arrange;

        if (!TryGetComponent<AudioSource>(out bgmSource))
            bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        bgmSource.loop = true;
        PlayBgm();
    }
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
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

    public void PlayBGM(string bgmName)
    {
        if (!addressBGMList.ContainsKey(bgmName)) return;
        bgmSource.Stop();
        bgmSource.clip = addressBGMList[bgmName];
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }
    
    public void FadeOutBGM(float duration = 1f)
    {
        if (!bgmSource.isPlaying) return; 
        bgmSource.DOFade(0f, duration).OnComplete(() => bgmSource.Stop());
    }
    
    public void FadeOutSFX(float duration = 1f)
    {
        sfxSource.DOFade(0f, duration).OnComplete(() => sfxSource.Stop());
    }


    public void PlaySFX(AudioClip sfx)
    {
        sfxSource.PlayOneShot(sfx, volumes[VOLTYPE.MASTER] * volumes[VOLTYPE.SFX]);
    }

    public void Playsfx(string sfxName)
    {
        if (addressSfxList.ContainsKey(sfxName))
        {
            PlaySFX(addressSfxList[sfxName]);
            return;
        }
        
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
    
    public void PlaySfxRepeat(string sfxName, float interval, int repeatCount = 1)
    {
        PlaySfxRepeatAsync(sfxName, interval, repeatCount).Forget();
    }

    private async UniTaskVoid PlaySfxRepeatAsync(string sfxName, float interval, int repeatCount)
    {
        int count = 0;
        while (repeatCount < 0 || count < repeatCount)
        {
            Playsfx(sfxName);
            count++;
            await UniTask.Delay(TimeSpan.FromSeconds(interval));
        }
    }


    public void ChangeSound(VOLTYPE voltype, float amount)
    {
        volumes[voltype] = amount;
        bgmSource.volume = volumes[VOLTYPE.MASTER] * volumes[VOLTYPE.BGM];
    }

    public void Reset()
    {
        bgmSource.volume = volumes[VOLTYPE.MASTER] * volumes[VOLTYPE.BGM];
    }
}
