using System.Collections.Generic;
using UnityEngine;


public enum EnemySoundType { Hit, Attack, Casting }

[CreateAssetMenu(menuName = "SO/Enemy/SoundSO")]
public class EnemySoundSO : ScriptableObject
{
    [System.Serializable] public class SoundData { public EnemySoundType soundType; public AudioClip clip; }

    public string enemyName;
    public List<SoundData> soundList;
}