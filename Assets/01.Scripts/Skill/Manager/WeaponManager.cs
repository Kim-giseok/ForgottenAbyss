using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    public SkillController skillController;
    public SpriteRenderer playerRenderer;

    private WeaponDataSO currentWeaponSO;
    private WeaponData currentWeaponData;

    public void EquipWeapon(WeaponDataSO selectedWeapon)
    {
        if (selectedWeapon == null)
        {
            Debug.LogWarning("무기 데이터 없음");
            return;
        }

        currentWeaponSO = selectedWeapon;

   
        currentWeaponData = DataManager.Instance.GetWeaponData(selectedWeapon.currentWeaponId);
        if (currentWeaponData == null)
        {
            Debug.LogWarning($"WeaponData not found for ID {selectedWeapon.currentWeaponId}");
        }

        // 스프라이트 변경
        playerRenderer.sprite = selectedWeapon.playerSprite;

        // 스킬 ID 설정
        skillController.basicAttackSkillId = selectedWeapon.basicAttack.skillId;
        skillController.skill01Id = selectedWeapon.skill01SO.skillId;
        skillController.skill02Id = selectedWeapon.skill02SO.skillId;

        SkillManager.Instance.SetCurrentWeaponSkills(currentWeaponData.Id);
    }

    public float GetCurrentWeaponAttack()
    {
        return currentWeaponData != null ? currentWeaponData.Damage : 0f;
    }

    public WeaponData GetCurrentWeaponData() => currentWeaponData;
    public WeaponDataSO GetCurrentWeaponSO() => currentWeaponSO;

#if UNITY_EDITOR
    [ContextMenu("DEBUG: 기본 무기 장착")]
    private void Debug_EquipTestWeapon()
    {
        var testWeaponSO = Resources.Load<WeaponDataSO>("Weapon/Sword_SO"); // 경로에 맞게 수정
        EquipWeapon(testWeaponSO);
    }
#endif
}
