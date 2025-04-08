using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public SkillController skillController;
    public SpriteRenderer playerRenderer;

    public void EquipWeapon(WeaponDataSO selectedWeapon)
    {
        if (selectedWeapon == null)
        {
            Debug.LogWarning("무기 데이터 없음");
            return;
        }

        // 스프라이트 변경
        playerRenderer.sprite = selectedWeapon.playerSprite;

        // 스킬 ID 설정
        skillController.basicAttackSkillId = selectedWeapon.basicAttack.skillId;
        skillController.skill01Id = selectedWeapon.skill01SO.skillId;
        skillController.skill02Id = selectedWeapon.skill02SO.skillId;
    }
}
