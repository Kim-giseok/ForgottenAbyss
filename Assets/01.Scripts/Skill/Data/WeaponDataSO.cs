using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Weapon")]
public class WeaponDataSO : ScriptableObject
{
    public int Id;

    // 무기와 이펙트 프리팹
    public GameObject WeaponPrefab;
    public GameObject AttackEffectPrefab;

    // 사운드 추가 시
    public AudioClip AttackSound;

    // 연결할 스킬 SO
    public SkillVisualSO Skill01SO;
    public SkillVisualSO Skill02SO;
}
