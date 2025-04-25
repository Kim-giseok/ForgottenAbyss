using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewComboAttackData", menuName = "SO/Combat/ComboAttackData")]
public class ComboAttackSO : ScriptableObject
{
    public int id;
    public string attackName;
    public string description;
    public Sprite icon;
    public List<ComboStep> comboSteps;

    public ComboAttackSO Clone()
    {
        ComboAttackSO clone = CreateInstance<ComboAttackSO>();

        clone.id = this.id;
        clone.attackName = this.attackName;
        clone.description = this.description;
        clone.icon = this.icon;

        clone.comboSteps = new List<ComboStep>();
        foreach (var step in this.comboSteps)
        {
            clone.comboSteps.Add(new ComboStep
            {
                animationName = step.animationName,
                damageMultiplier = step.damageMultiplier,
                moveDistance = step.moveDistance,
                inputBufferTime = step.inputBufferTime,
                radius = step.radius,
                offset = step.offset
            });
        }

        return clone;
    }
}

[System.Serializable]
public class ComboStep
{
    public string animationName;    // 애니메이션 이름
    public float damageMultiplier;  // 데미지 배율
    public float moveDistance;      // 공격 시 앞으로 이동할 거리
    public float inputBufferTime;   // 다음 입력을 받을 수 있는 시간
    public float radius;
    public float offset;
}
