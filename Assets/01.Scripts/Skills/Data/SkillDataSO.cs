using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO", menuName = "SO/SkillSO")]
public class SkillDataSO : ScriptableObject
{
    // 이펙트 프리팹
    public GameObject skillEffectPrefab;

    // 사운드 추가 시
    public AudioClip skillSound;

    // 필요한 스탯 추가 (유지 시간, 사거리 이런거?)
}
