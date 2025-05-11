using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// resource 또는 SoundSO로 관리 필요
public class EnemySoundHandler: MonoBehaviour
{
    [FormerlySerializedAs("soundSO")] public EnemySoundsSO soundsSO;
    
    public AudioClip GetClip(EnemySoundType type)
    {
        if (!soundsSO) return null;
        
        int index = (int)type;
        if (index >= 0 && index < soundsSO.soundList.Count) return soundsSO.soundList[index]?.clip;
        return null;
    }

    public void Play(EnemySoundType soundType)
    {
        if (!SoundManager.Instance) return;
        SoundManager.Instance.PlaySFX(GetClip(soundType));
    }
}