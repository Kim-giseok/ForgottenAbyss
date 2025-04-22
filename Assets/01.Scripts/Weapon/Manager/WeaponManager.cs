using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponManager : Singleton<WeaponManager>
{
    public SkillController skillController;
    public ComboAttack comboAttack; 
    public WeaponSwapper swapper;

    private WeaponDataSO currentWeaponSO;
    private WeaponData currentWeaponData;

    private MemoryPieceData currentMemoryData;
    private MemoryPieceSO currentMemorySO;

    private SkillUI skillUI;
    private int previousWeaponId = -1;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        skillUI = FindObjectOfType<SkillUI>();
        skillController = FindObjectOfType<SkillController>();
        comboAttack = FindObjectOfType<ComboAttack>();
        if(swapper == null)
            swapper = FindObjectOfType<WeaponSwapper>();

        RefreshSkillController();
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => skillUI.IsInitialized);
        LoadWeaponState();
        //EquipDefaultWeapons();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Debug_EquipTestWeapon();  // 검 장착
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Debug_EquipTestBow();     // 활 장착
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug_EquipTestMemoryPiece(); // 기억 조각 장착
        }
#endif
    }

    private void OnApplicationQuit()
    {
        SaveWeaponState();
    }

    public void EquipDefaultWeapons()
    {
        var defaultSwordSO = Resources.Load<WeaponDataSO>("Weapon/Sword_SO");
        var defaultBowSO = Resources.Load<WeaponDataSO>("Weapon/Bow_SO");
        if (defaultSwordSO != null)
        {
            EquipWeapon(defaultSwordSO);

            swapper.SetWeaponIcons(defaultSwordSO.weaponIcon, defaultBowSO.weaponIcon);
        }
        else
        {
            Debug.LogWarning("기본 검 무기 SO를 찾을 수 없습니다.");
        }

        //var defaultMemorySO = Resources.Load<MemoryPieceSO>("Weapon/MemoryPieceSO");
        //if (defaultMemorySO != null)
        //{
        //    EquipMemoryPiece(defaultMemorySO);
        //}
        //else
        //{
        //    Debug.LogWarning("기본 기억 조각 SO를 찾을 수 없습니다.");
        //}
    }

    public void EquipWeapon(WeaponDataSO selectedWeapon)
    {
        if (selectedWeapon == null)
        {
            Debug.LogWarning("무기 데이터 없음");
            return;
        }

        if (currentWeaponSO != null && skillUI != null)
        {
            previousWeaponId = currentWeaponSO.currentWeaponId;
            skillUI.SaveCurrentCooldown(previousWeaponId);
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

        skillUI.LoadCooldownFromWeapon(selectedWeapon.currentWeaponId);

        if (currentWeaponData.Type == WeaponType.Sword && selectedWeapon.comboAttackData != null)
        {
            skillController.combatId = selectedWeapon.comboAttackData.id;
            skillController.comboAttack.SetComboData(selectedWeapon.comboAttackData);
            if (skillUI != null)
                skillUI.SetSkillIcon(SkillSlotType.Basic, selectedWeapon.comboAttackData.icon);
        }
        else if (currentWeaponData.Type == WeaponType.Bow && selectedWeapon.rangedAttackData != null)
        {
            skillController.combatId = selectedWeapon.rangedAttackData.id;
            skillController.rangedAttack.SetRangedAttackData(selectedWeapon.rangedAttackData);
            if (skillUI != null)
                skillUI.SetSkillIcon(SkillSlotType.Basic, selectedWeapon.rangedAttackData.icon);
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

        skillController.memorySkillItem = currentMemorySO.skillItem;

        SkillManager.Instance.SetMemorySkill(memorySO);

        if (skillUI == null)
            skillUI = FindObjectOfType<SkillUI>();

        skillUI.SetSkillIcon(SkillSlotType.Memory, memorySO.icon);

        if (memorySO.skillItem != null)
        {
            skillUI.SetSkillCooldownTime(SkillSlotType.Memory, memorySO.skillItem.coolTime);
        }
        else
        {
            Debug.LogWarning("SkillItem이 비어 있어 쿨타임을 설정할 수 없습니다.");
        }
    }

    public float GetCurrentWeaponAttack()
    {
        return currentWeaponData != null ? currentWeaponData.Damage : 0f;
    }

    public WeaponData GetCurrentWeaponData() => currentWeaponData;
    public WeaponDataSO GetCurrentWeaponSO() => currentWeaponSO;

    public MemoryPieceData GetCurrentMemoryPieceData() => currentMemoryData;
    public MemoryPieceSO GetCurrentMemoryPieceSO() => currentMemorySO;

    public void SwapWeapon()
    {
        if (currentWeaponData == null)
        {
            Debug.LogWarning("현재 무기 데이터가 없습니다.");
            return;
        }

        string nextWeaponPath = "";

        switch (currentWeaponData.Type)
        {
            case WeaponType.Sword:
                nextWeaponPath = "Weapon/Bow_SO";
                break;
            case WeaponType.Bow:
                nextWeaponPath = "Weapon/Sword_SO";
                break;
            default:
                Debug.LogWarning("스왑할 수 없는 무기 타입입니다.");
                return;
        }

        var nextWeaponSO = Resources.Load<WeaponDataSO>(nextWeaponPath);
        if (nextWeaponSO != null)
            EquipWeapon(nextWeaponSO);
        else
            Debug.LogWarning($"경로에 무기 SO 없음: {nextWeaponPath}");

        swapper.SwapWeapons();
    }

    public void RefreshSkillController()
    {
        if (skillController == null) return;

        if (currentWeaponSO != null)
        {
            skillController.skill01Id = currentWeaponSO.skill01SO.skillId;
            skillController.skill02Id = currentWeaponSO.skill02SO.skillId;

            if (currentWeaponData == null)
            {
                currentWeaponData = DataManager.Instance.GetWeaponData(currentWeaponSO.currentWeaponId);
            }

            if (currentWeaponData.Type == WeaponType.Sword && currentWeaponSO.comboAttackData != null)
            {
                skillController.combatId = currentWeaponSO.comboAttackData.id;
                skillController.comboAttack.SetComboData(currentWeaponSO.comboAttackData);
            }
            else if (currentWeaponData.Type == WeaponType.Bow && currentWeaponSO.rangedAttackData != null)
            {
                skillController.combatId = currentWeaponSO.rangedAttackData.id;
                skillController.rangedAttack.SetRangedAttackData(currentWeaponSO.rangedAttackData);
            }
        }

        if (currentMemorySO != null)
        {
            skillController.memorySkillItem = currentMemorySO.skillItem;
        }
    }

    public void SaveWeaponState()
    {
        var saveData = new WeaponSaveData
        {
            weaponId = currentWeaponSO != null ? currentWeaponSO.currentWeaponId : -1,
            memoryPieceId = currentMemorySO != null ? currentMemorySO.currentMemoryPieceId : -1
        };

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("WeaponSaveData", json);
        PlayerPrefs.Save();

        Debug.Log("[Save] 무기 및 기억 조각 상태 저장 완료");
    }

    public void LoadWeaponState()
    {
        string json = PlayerPrefs.GetString("WeaponSaveData", null);
        if (string.IsNullOrEmpty(json)) return;

        WeaponSaveData saveData = JsonUtility.FromJson<WeaponSaveData>(json);

        if (saveData.weaponId != -1)
        {
            WeaponDataSO weaponSO = DataManager.Instance.weaponSOList.Find(w => w.currentWeaponId == saveData.weaponId);
            if (weaponSO != null)
            {
                EquipWeapon(weaponSO);
                Debug.Log("[Load] 무기 로드");

                if (swapper == null)
                    swapper = FindObjectOfType<WeaponSwapper>();

                var swordSO = DataManager.Instance.weaponSOList.Find(w =>
                                      DataManager.Instance.GetWeaponData(w.currentWeaponId).Type == WeaponType.Sword);

                var bowSO = DataManager.Instance.weaponSOList.Find(w =>
                                   DataManager.Instance.GetWeaponData(w.currentWeaponId).Type == WeaponType.Bow);


                if (swordSO != null && bowSO != null)
                {
                    if (currentWeaponSO == swordSO)
                        swapper.SetWeaponIcons(swordSO.weaponIcon, bowSO.weaponIcon);
                    else
                        swapper.SetWeaponIcons(bowSO.weaponIcon, swordSO.weaponIcon);

                    Debug.Log($"[SetIcon] 무기 스왑 아이콘 설정 완료 (현재 장착: {currentWeaponSO.name})");
                }
                else
                {
                    Debug.LogWarning("[SetIcon] 무기 타입 기반으로 아이콘 설정 실패 - Sword 또는 Bow SO 없음");
                }
            }
        }

        if (saveData.memoryPieceId != -1)
        {
            MemoryPieceSO memorySO = DataManager.Instance.memoryVisualSOList.Find(m => m.currentMemoryPieceId == saveData.memoryPieceId);
            if (memorySO != null)
            {
                EquipMemoryPiece(memorySO);
                Debug.Log("[Load] 기억 조각 로드");
            }
        }

        Debug.Log("[Load] 무기 및 기억 조각 상태 로드 완료");
    }

#if UNITY_EDITOR
    [ContextMenu("DEBUG: 검 장착")]
    private void Debug_EquipTestWeapon()
    {
        var testWeaponSO = Resources.Load<WeaponDataSO>("Weapon/Sword_SO");
        EquipWeapon(testWeaponSO);
    }

    [ContextMenu("DEBUG: 활 장착")]
    private void Debug_EquipTestBow()
    {
        var testWeaponSO = Resources.Load<WeaponDataSO>("Weapon/Bow_SO");
        EquipWeapon(testWeaponSO);
    }

    [ContextMenu("DEBUG: 기억 조각 장착")]
    private void Debug_EquipTestMemoryPiece()
    {
        var testMemorySO = Resources.Load<MemoryPieceSO>("Weapon/MemoryPiece01_SO");
        EquipMemoryPiece(testMemorySO);
    }
#endif

    [Serializable]
    private class WeaponSaveData
    {
        public int weaponId;
        public int memoryPieceId;
    }
}
