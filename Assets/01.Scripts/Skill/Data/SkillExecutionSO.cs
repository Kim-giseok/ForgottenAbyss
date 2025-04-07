using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillExecutionSO", menuName = "SO/Skill/Execution")]
public class SkillExecutionSO : ScriptableObject
{
    [TextArea]
    public string description;

    // 실행할 로직을 여기에 정의 (예시로)
    public virtual void Execute(GameObject caster, GameObject target)
    {
        Debug.Log($"[SkillExecutionSO] {name} 실행됨");
    }
}
