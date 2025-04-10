using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    public SkillController skillController;
    public ComboAttack comboAttack; 

    private WeaponDataSO currentWeaponSO;
    private WeaponData currentWeaponData;

    private MemoryPieceData currentMemoryData;
    private MemoryPieceSO currentMemorySO;

    private SkillUI skillUI;

    private void Awake()
    {
        if (skillUI == null)
            skillUI = FindObjectOfType<SkillUI>();
    }

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

        // 스킬 ID 설정
        skillController.skill01Id = selectedWeapon.skill01SO.skillId;
        skillController.skill02Id = selectedWeapon.skill02SO.skillId;

        SkillManager.Instance.SetCurrentWeaponSkills(currentWeaponData.Id);

        if (skillUI == null)
            skillUI = FindObjectOfType<SkillUI>();

        skillUI.SetSkillIcon(SkillSlotType.Skill01, selectedWeapon.skill01SO.skillIcon);
        skillUI.SetSkillIcon(SkillSlotType.Skill02, selectedWeapon.skill02SO.skillIcon);

        var skill01Data = DataManager.Instance.GetSkillData(selectedWeapon.skill01SO.skillId);
        var skill02Data = DataManager.Instance.GetSkillData(selectedWeapon.skill02SO.skillId);

        skillUI.SetSkillCooldownTime(SkillSlotType.Skill01, skill01Data.CoolTime);
        skillUI.SetSkillCooldownTime(SkillSlotType.Skill02, skill02Data.CoolTime);

        if (skillController.comboAttack != null && selectedWeapon.comboAttackData != null)
        {
            skillController.comboAttack.SetComboData(selectedWeapon.comboAttackData);
            if (skillUI != null)
            {
                skillUI.SetSkillIcon(SkillSlotType.Basic, selectedWeapon.comboAttackData.icon);
            }
        }
    }

    public void EquipMemoryPiece(MemoryPieceSO memorySO)
    {
        if (memorySO == null)
        {
            Debug.LogWarning("기억 조각 데이터 없음");
            return;
        }

        currentMemorySO = memorySO;
        currentMemoryData = DataManager.Instance.GetMemoryPieceData(memorySO.currentMemoryPieceId);

        if (currentMemoryData == null)
        {
            Debug.LogWarning($"MemoryPieceData not found for ID {memorySO.currentMemoryPieceId}");
            return;
        }

        skillController.memorySkillId = memorySO.memorySkillVisualSO.skillId;

        SkillManager.Instance.SetMemorySkill(currentMemorySO.memorySkillVisualSO.skillId);

        if (skillUI == null)
            skillUI = FindObjectOfType<SkillUI>();

        skillUI.SetSkillIcon(SkillSlotType.Memory, memorySO.memorySkillVisualSO.skillIcon);

        var memoryData = DataManager.Instance.GetSkillData(memorySO.memorySkillVisualSO.skillId);

        skillUI.SetSkillCooldownTime(SkillSlotType.Memory, memoryData.CoolTime);
    }

    public float GetCurrentWeaponAttack()
    {
        return currentWeaponData != null ? currentWeaponData.Damage : 0f;
    }

    public WeaponData GetCurrentWeaponData() => currentWeaponData;
    public WeaponDataSO GetCurrentWeaponSO() => currentWeaponSO;

    public MemoryPieceData GetCurrentMemoryPieceData() => currentMemoryData;
    public MemoryPieceSO GetCurrentMemoryPieceSO() => currentMemorySO;

#if UNITY_EDITOR
    [ContextMenu("DEBUG: 기본 무기 장착")]
    private void Debug_EquipTestWeapon()
    {
        var testWeaponSO = Resources.Load<WeaponDataSO>("Weapon/Sword_SO");
        EquipWeapon(testWeaponSO);
    }

    [ContextMenu("DEBUG: 기억 조각 장착")]
    private void Debug_EquipTestMemoryPiece()
    {
        var testMemorySO = Resources.Load<MemoryPieceSO>("Weapon/MemoryPieceSO");
        EquipMemoryPiece(testMemorySO);
    }
#endif
}
