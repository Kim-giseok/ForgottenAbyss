using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillExecutionSO", menuName = "SO/Skill/Execution")]
public class SkillExecutionSO : ScriptableObject
{
    [TextArea]
    public string description;

    // 스킬 실행
    public virtual void Execute(GameObject caster, GameObject target)
    {
        Debug.Log($"[SkillExecutionSO] {name} 실행됨");
    }
}
