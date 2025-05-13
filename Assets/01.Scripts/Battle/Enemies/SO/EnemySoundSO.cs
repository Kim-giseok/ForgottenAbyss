using System.Collections.Generic;
using UnityEngine;


public enum EnemySoundType { Hit, Attack, Casting }

[CreateAssetMenu(menuName = "SO/Enemy/SoundSO")]
public class EnemySoundSO : EnemySO
{
    [System.Serializable] public class SoundData { public EnemySoundType soundType; public AudioClip clip; }
    public List<SoundData> soundList;
}