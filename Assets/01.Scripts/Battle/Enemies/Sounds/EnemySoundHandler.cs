using System.Collections.Generic;
using UnityEngine;

// resource 또는 SoundSO로 관리 필요
public class EnemySoundHandler: MonoBehaviour
{
    public EnemySoundSO soundSO;
    
    public AudioClip GetClip(EnemySoundType type)
    {
        if (!soundSO) return null;
        
        int index = (int)type;
        if (index >= 0 && index < soundSO.soundList.Count) return soundSO.soundList[index]?.clip;
        return null;
    }
}