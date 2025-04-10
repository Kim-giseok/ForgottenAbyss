using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/Weapon")]
public class WeaponDataSO : ScriptableObject
{
    // ������ ����id
    public int currentWeaponId = 0;

    // ������ ��ų SO
    public SkillVisualSO basicAttack;
    public SkillVisualSO skill01SO;
    public SkillVisualSO skill02SO;

    // ���� ������
    public Sprite weaponIcon;
    public Sprite playerSprite;
}
