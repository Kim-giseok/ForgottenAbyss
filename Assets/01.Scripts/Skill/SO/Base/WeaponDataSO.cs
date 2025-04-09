using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Weapon")]
public class WeaponDataSO : ScriptableObject
{
    // 연결할 무기id
    public int currentWeaponId = 0;

    // 연결할 스킬 SO
    public SkillVisualSO basicAttack;
    public SkillVisualSO skill01SO;
    public SkillVisualSO skill02SO;

    // 무기 아이콘
    public Sprite weaponIcon;
    public Sprite playerSprite;
}
