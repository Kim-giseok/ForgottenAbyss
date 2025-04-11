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
}

[System.Serializable]
public class RangedComboStep
{
    public string animationName;        // 재생할 애니메이션 이름
    public GameObject projectilePrefab; // 발사할 투사체 프리팹
    public int projectileCount = 1;     // 몇 개 발사할지
    public float fireDelay = 0.1f;      // 투사체 간 딜레이
    public float firePower = 1f;        // 투사체 힘 (속도 등)
    public bool isSpread = false;
    public float spreadAngle = 15f;
}