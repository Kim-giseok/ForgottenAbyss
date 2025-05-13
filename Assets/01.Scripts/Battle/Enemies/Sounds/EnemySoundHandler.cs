using System.Collections.Generic;
using UnityEngine;

// resource 또는 SoundSO로 관리 필요
public class EnemySoundHandler: MonoBehaviour
{
    private EnemySoundSO _soundSO;
    public void Define(EnemySoundSO newSoundSO) => _soundSO = newSoundSO;
    
    private AudioClip GetClip(EnemySoundType type)
    {
        if (!_soundSO) return null;
        
        int index = (int)type;
        if (index >= 0 && index < _soundSO.soundList.Count) return _soundSO.soundList[index]?.clip;
        return null;
    }

    public void Play(EnemySoundType soundType)
    {
        if (!SoundManager.Instance) return;
        var currClip = GetClip(soundType);
        if (!currClip) return;
        SoundManager.Instance.PlaySFX(currClip);
    }
}