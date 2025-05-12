using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedAttackData", menuName = "SO/Combat/RangedAttackData")]
public class RangedAttackSO : ScriptableObject
{
    public int id;
    public string attackName;
    public string description;
    public Sprite icon;

    public List<RangedComboStep> comboSteps = new();

    public RangedAttackSO Clone()
    {
        RangedAttackSO clone = CreateInstance<RangedAttackSO>();

        clone.id = this.id;
        clone.attackName = this.attackName;
        clone.description = this.description;
        clone.icon = this.icon;

        clone.comboSteps = new List<RangedComboStep>();
        foreach (var step in this.comboSteps)
        {
            clone.comboSteps.Add(new RangedComboStep
            {
                animationName = step.animationName,
                projectilePrefab = step.projectilePrefab,
                projectileCount = step.projectileCount,
                fireDelay = step.fireDelay,
                multiplier = step.multiplier,
                isSpread = step.isSpread,
                spreadAngle = step.spreadAngle,
                stepIcon = step.stepIcon
            });
        }

        return clone;
    }
}

[System.Serializable]
public class RangedComboStep
{
    public string animationName;        // 재생할 애니메이션 이름
    public GameObject projectilePrefab; // 발사할 투사체 프리팹
    public int projectileCount = 1;     // 몇 개 발사할지
    public float fireDelay = 0.1f;      // 투사체 간 딜레이
    public float multiplier = 1f;        // 계수
    public bool isSpread = false;
    public float spreadAngle = 15f;
    public Sprite stepIcon;
}