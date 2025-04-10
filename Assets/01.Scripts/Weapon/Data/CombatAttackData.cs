using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComboAttackData
{
    public List<ComboStep> comboSteps;
}

[System.Serializable]
public class ComboStep
{
    public string animationName;    // 애니메이션 이름
    public float damageMultiplier;  // 데미지 배율
    public float moveDistance;      // 공격 시 앞으로 이동할 거리
    public float inputBufferTime;   // 다음 입력을 받을 수 있는 시간
}
