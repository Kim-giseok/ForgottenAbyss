using System.Collections.Generic;
using UnityEngine;


public enum EnemySoundType { Hit, Attack }

[CreateAssetMenu(menuName = "SO/EnemySoundSO")]
public class EnemySoundSO : ScriptableObject
{
    [System.Serializable] public class SoundData { public EnemySoundType soundType; public AudioClip clip; }

    public List<SoundData> soundList;
}