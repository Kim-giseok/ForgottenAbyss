using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VisualSkillSO", menuName = "SO/Skill/VisualSkillSO")]
public class SkillVisualSO : ScriptableObject
{
    public int skillId;

    // 스킬 실행 연결
    public SkillExecutionSO executionSO;

    // 스킬 아이콘
    public Sprite skillIcon;

    // 이펙트 프리팹
    public GameObject skillEffectPrefab;

    // 사운드 추가 시
    public AudioClip skillSound;

    // 연결 애니메이션
    public string animationName;
    public float animationSpeed;
    public float resetTime;

    // 이펙트 풀 키값
    public string effectKey;

    // 이펙트 위치 오프셋 사용 여부 + 값
    public bool useEffectOffset = false;
    public float effectXOffset = 0f;
    public float effectYOffset = 0f;

    // 이펙트 딜레이 시간
    public float effectDelay;

    public bool isTogether;
}
