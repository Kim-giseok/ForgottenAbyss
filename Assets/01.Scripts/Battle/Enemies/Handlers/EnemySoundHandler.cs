using System.Collections.Generic;
using UnityEngine;

// resource 또는 SoundSO로 관리 필요
public class EnemySoundHandler: MonoBehaviour
{
    public enum SoundType { Hit, Attack }
    [System.Serializable] public class SoundData { public SoundType soundType; public AudioClip clip; }

    public List<SoundData> soundList;
    
    public AudioClip GetClip(EnemySoundHandler.SoundType type)
    {
        int index = (int)type;
        if (index >= 0 && index < soundList.Count) return soundList[index]?.clip;
        return null;
    }
}