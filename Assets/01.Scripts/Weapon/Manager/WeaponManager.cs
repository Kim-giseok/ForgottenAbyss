using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponManager : MonoBehaviour
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

    private List<SkillInstance> weaponSkillInstances = new();
    private SkillInstance memorySkillInstance;

    private DataManager dataManager;

    private const string WEAPON_DATA_FILE = "WeaponSaveData.json";

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
        skillController = FindObjectOfType<SkillController>();
        comboAttack = FindObjectOfType<ComboAttack>();

        if(swapper == null)
            swapper = FindObjectOfType<WeaponSwapper>();

        if(skillUI == null)
            skillUI = FindObjectOfType<SkillUI>();

        dataManager = SystemManager.Instance.dataManager;

        RefreshSkillController();
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => dataManager.IsInitialized);
        yield return new WaitUntil(() => skillUI.IsInitialized);

        LoadWeaponState();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ClearWeaponSaveData();
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
        currentWeaponData = dataManager.GetWeaponData(selectedWeapon.currentWeaponId);

        if (currentWeaponData == null)
        {
            Debug.LogWarning($"WeaponData not found for ID {selectedWeapon.currentWeaponId}");
        }

        // 스킬 등록
        weaponSkillInstances.Clear();
        var skill01Data = dataManager.GetSkillData(selectedWeapon.skill01SO.skillId);
        var skill02Data = dataManager.GetSkillData(selectedWeapon.skill02SO.skillId);

        var skillInstance01 = new SkillInstance(skill01Data).Clone();
        var skillInstance02 = new SkillInstance(skill02Data).Clone();

        weaponSkillInstances.Add(skillInstance01);
        weaponSkillInstances.Add(skillInstance02);

        // UI 연동
        if (skillUI == null)
            skillUI = FindObjectOfType<SkillUI>();

        skillUI.SetSkillIcon(SkillSlotType.Skill01, skillInstance01.GetIcon());
        skillUI.SetSkillIcon(SkillSlotType.Skill02, skillInstance02.GetIcon());
        skillUI.SetSkillCooldownTime(SkillSlotType.Skill01, skillInstance01.GetCooldown());
        skillUI.SetSkillCooldownTime(SkillSlotType.Skill02, skillInstance02.GetCooldown());
        skillUI.LoadCooldownFromWeapon(selectedWeapon.currentWeaponId);

        // 무기 타입에 따른 기본 공격 세팅
        if (currentWeaponData.Type == WeaponType.Sword && selectedWeapon.comboAttackData != null)
        {
            skillController.comboAttack.SetComboData(selectedWeapon.comboAttackData);
            skillController.combatSkill = new CombatInstance(skillController.comboAttack, selectedWeapon.comboAttackData).Clone();
            skillUI.SetSkillIcon(SkillSlotType.Basic, selectedWeapon.comboAttackData.icon);
        }
        else if (currentWeaponData.Type == WeaponType.Bow && selectedWeapon.rangedAttackData != null)
        {
            skillController.rangedAttack.SetRangedAttackData(selectedWeapon.rangedAttackData);
            skillController.combatSkill = new CombatInstance(skillController.rangedAttack, selectedWeapon.rangedAttackData).Clone();
            skillUI.SetSkillIcon(SkillSlotType.Basic, selectedWeapon.rangedAttackData.icon);
        }

        SystemManager.Instance.skillManager.SetCurrentWeaponSkills(selectedWeapon.currentWeaponId);
        GameManager.Instance.player.controller.UpdateDashValues(currentWeaponData.Type);
    }

    public void EquipMemoryPiece(MemoryPieceSO memorySO)
    {
        if (memorySO == null)
        {
            Debug.LogWarning("기억 조각 데이터 없음");
            return;
        }

        if (currentMemorySO != null && currentMemorySO.currentMemoryPieceId == memorySO.currentMemoryPieceId)
        {
            UnequipMemoryPiece();
            return;
        }

        currentMemorySO = memorySO;
        currentMemoryData = dataManager.GetMemoryPieceData(memorySO.currentMemoryPieceId);

        var memory = new SkillInstance(memorySO).Clone();
        memorySkillInstance = memory;

        SystemManager.Instance.skillManager.SetMemorySkill(memorySkillInstance.memorySO);

        skillUI.SetSkillIcon(SkillSlotType.Memory, memorySkillInstance.GetIcon());
        skillUI.SetSkillCooldownTime(SkillSlotType.Memory, memorySkillInstance.GetCooldown());
    }

    public void UnequipMemoryPiece()
    {
        if (currentMemorySO == null)
        {
            Debug.LogWarning("해제할 기억 조각이 없습니다.");
            return;
        }

        // 메모리 관련 정보 초기화
        currentMemorySO = null;
        currentMemoryData = null;
        memorySkillInstance = null;

        // SkillManager에서 기억 스킬 제거
        SystemManager.Instance.skillManager.SetMemorySkill(null);

        // UI 초기화
        skillUI.ClearSkillIcon(SkillSlotType.Memory);

        Debug.Log("기억 조각 해제 확인용");
    }

    public float GetCurrentWeaponAttack()
    {
        return currentWeaponData != null ? currentWeaponData.Damage : 0f;
    }

    public bool IsWeaponEquipped()
    {
        return currentWeaponData != null;
    }

    public WeaponData GetCurrentWeaponData() => currentWeaponData;
    public WeaponDataSO GetCurrentWeaponSO() => currentWeaponSO;

    public MemoryPieceData GetCurrentMemoryPieceData() => currentMemoryData;
    public MemoryPieceSO GetCurrentMemoryPieceSO() => currentMemorySO;

    public SkillInstance GetWeaponSkillInstance(int index)
    {
        if (index < 0 || index >= weaponSkillInstances.Count) return null;
        return weaponSkillInstances[index];
    }

    public SkillInstance GetMemorySkillInstance()
    {
        return memorySkillInstance;
    }

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

        skillController.ResetAttack();

        var nextWeaponSO = Resources.Load<WeaponDataSO>(nextWeaponPath);
        if (nextWeaponSO != null)
            EquipWeapon(nextWeaponSO);
        else
            Debug.LogWarning($"경로에 무기 SO 없음: {nextWeaponPath}");

        swapper.SwapWeapons();
        skillController.UpdateBasicIcon(); 
    }

    public void RefreshSkillController()
    {
        if (skillController == null) return;

        if (currentWeaponSO != null)
        {
            if (currentWeaponData == null)
                currentWeaponData = dataManager.GetWeaponData(currentWeaponSO.currentWeaponId);

            if (currentWeaponData.Type == WeaponType.Sword && currentWeaponSO.comboAttackData != null)
            {
                skillController.comboAttack.SetComboData(currentWeaponSO.comboAttackData);
            }
            else if (currentWeaponData.Type == WeaponType.Bow && currentWeaponSO.rangedAttackData != null)
            {
                skillController.rangedAttack.SetRangedAttackData(currentWeaponSO.rangedAttackData);
            }
        }
    }

    public void SaveWeaponState()
    {
        var saveData = new WeaponSaveData
        {
            weaponId = currentWeaponSO != null ? currentWeaponSO.currentWeaponId : -1,
            memoryPieceId = currentMemorySO != null ? currentMemorySO.currentMemoryPieceId : -1
        };

        DataSave<WeaponSaveData>.SaveData(saveData, WEAPON_DATA_FILE);

        Debug.Log("[Save] 무기 및 기억 조각 상태 저장 완료");
    }

    public void LoadWeaponState()
    {
        WeaponSaveData saveData = DataSave<WeaponSaveData>.LoadData(WEAPON_DATA_FILE);

        if (saveData != null)
        {
            if (saveData.weaponId != -1)
            {
                WeaponDataSO weaponSO = dataManager.weaponSOList.Find(w => w.currentWeaponId == saveData.weaponId);
                if (weaponSO != null)
                {
                    EquipWeapon(weaponSO);
                    Debug.Log("[Load] 무기 로드");

                    // Swapper 및 아이콘 설정 
                    if (swapper == null)
                        swapper = FindObjectOfType<WeaponSwapper>();

                    var swordSO = dataManager.weaponSOList.Find(w =>
                                          dataManager.GetWeaponData(w.currentWeaponId).Type == WeaponType.Sword);

                    var bowSO = dataManager.weaponSOList.Find(w =>
                                       dataManager.GetWeaponData(w.currentWeaponId).Type == WeaponType.Bow);

                    if (swordSO != null && bowSO != null)
                    {
                        if (currentWeaponSO == swordSO)
                            swapper.SetWeaponIcons(swordSO.weaponIcon, bowSO.weaponIcon);
                        else
                            swapper.SetWeaponIcons(bowSO.weaponIcon, swordSO.weaponIcon);

                        Debug.Log($"[SetIcon] 무기 스왑 아이콘 설정 완료 (현재 장착: {currentWeaponSO.name})");
                    }
                    else
                        Debug.LogWarning("[SetIcon] 무기 타입 기반으로 아이콘 설정 실패 - Sword 또는 Bow SO 없음");
                }
            }

            if (saveData.memoryPieceId != -1)
            {
                MemoryPieceSO memorySO = dataManager.memoryVisualSOList.Find(m => m.currentMemoryPieceId == saveData.memoryPieceId);
                if (memorySO != null)
                {
                    EquipMemoryPiece(memorySO);
                    Debug.Log("[Load] 기억 조각 로드");
                }
            }

            Debug.Log("[Load] 무기 및 기억 조각 상태 로드 완료");
        }
    }

    public void UnequipWeapon()
    {
        currentWeaponSO = null;
        currentWeaponData = null;

        skillController.skill01 = null;
        skillController.skill02 = null;

        if (skillUI != null)
        {
            skillUI.ClearSkillIcon(SkillSlotType.Skill01);
            skillUI.ClearSkillIcon(SkillSlotType.Skill02);
            skillUI.ClearSkillIcon(SkillSlotType.Basic);
        }

        if (swapper != null)
            swapper.ClearWeaponIcons();

        Debug.Log("[Clear] 무기 장착 해제 완료");
    }

    //public void UnequipMemoryPiece()
    //{
    //    currentMemorySO = null;
    //    currentMemoryData = null;

    //    skillUI.ClearSkillIcon(SkillSlotType.Memory);

    //    Debug.Log("[Clear] 기억 조각 장착 해제 완료");
    //}

    public void ClearWeaponSaveData()
    {
        string path = Path.Combine(Application.persistentDataPath, WEAPON_DATA_FILE);

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("[Clear] 무기 및 기억 조각 저장 데이터 초기화 완료");
        }
        else
        {
            Debug.LogWarning("[Clear] 초기화 실패 - 파일 없음");
        }

        UnequipWeapon();
        UnequipMemoryPiece();
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
