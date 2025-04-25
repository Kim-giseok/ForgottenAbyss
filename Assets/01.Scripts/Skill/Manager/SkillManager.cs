using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    private Dictionary<int, float> nextAvailableTimes = new();
    public Dictionary<int, SkillInstance> skillInstances = new();

    public MemoryPieceSO currentMemoryPiece;

    public SkillUI skillUI;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (skillUI == null)
            skillUI = FindObjectOfType<SkillUI>();
    }

    public void SetCurrentWeaponSkills(int weaponId)
    {
        ClearSkill();

        var weaponData = DataManager.Instance.GetWeaponData(weaponId);
        if (weaponData == null) return;

        TryAddSkillInstance(weaponData.Skill1Id);
        TryAddSkillInstance(weaponData.Skill2Id);

        var controller = SkillController.Instance;
        if (controller != null)
        {
            controller.skill01 = GetSkillInstance(weaponData.Skill1Id);
            controller.skill02 = GetSkillInstance(weaponData.Skill2Id);
        }

        if (currentMemoryPiece != null)
        {
            SetMemorySkill(currentMemoryPiece);
        }
    }

    public void SetMemorySkill(MemoryPieceSO memorySO)
    {
        if (memorySO == null || memorySO.skillItem == null) return;

        int memoryId = memorySO.currentMemoryPieceId;

        if (!skillInstances.ContainsKey(memoryId))
            skillInstances[memoryId] = new SkillInstance(memorySO).Clone();

        currentMemoryPiece = memorySO;
        Debug.Log(currentMemoryPiece.name);

        TryAddSkillInstance(currentMemoryPiece.currentMemoryPieceId);
        var controller = SkillController.Instance;
        if (controller != null)
        {
            controller.memorySkill = GetSkillInstance(memoryId);
            Debug.Log($"[MemorySkill] 등록 완료: {controller.memorySkill.memorySO.displayName}");
        }
    }

    private void ClearSkill()
    {
        SkillInstance memorySkill = null;
        if (currentMemoryPiece != null)
        {
            int memoryId = currentMemoryPiece.currentMemoryPieceId;
            skillInstances.TryGetValue(memoryId, out memorySkill);
        }

        skillInstances.Clear();

        if (memorySkill != null && currentMemoryPiece != null)
        {
            skillInstances[currentMemoryPiece.currentMemoryPieceId] = memorySkill;
        }
    }

    private void TryAddSkillInstance(int skillId)
    {
        if (DataManager.Instance.HasSkillData(skillId))
        {
            var data = DataManager.Instance.GetSkillData(skillId);
            skillInstances[skillId] = new SkillInstance(data).Clone();
        }
    }

    public bool IsSkillEquipped(SkillInstance instance)
    {
        if (instance == null)
        {
            Debug.LogWarning("SkillInstance가 null입니다.");
            return false;
        }

        bool isMemory = currentMemoryPiece != null &&
                        currentMemoryPiece.skillItem != null &&
                        currentMemoryPiece.skillItem.memoryPieceId == instance.skillId;

        bool isWeaponSkill = skillInstances.ContainsKey(instance.skillId);

        return isMemory || isWeaponSkill;
    }

    private bool IsOnCooldown(SkillInstance instance)
    {
        if (nextAvailableTimes.TryGetValue(instance.skillId, out float nextTime) && Time.time < nextTime)
        {
            Debug.Log($"Skill {instance.skillId} is on cooldown. {nextTime - Time.time:F1}s left.");
            return true;
        }
        return false;
    }

    public void TryUseSkill(SkillInstance instance, Transform spawnPoint)
    {
        if (!IsSkillEquipped(instance)) return;
        if (IsOnCooldown(instance)) return;
        if (!IsEnoughCost(instance)) return;

        StartCoroutine(ExecuteSkill(instance, spawnPoint));
        UpdateCooldown(instance);
    }

    private IEnumerator ExecuteSkill(SkillInstance instance, Transform spawnPoint)
    {
        Animator anim = spawnPoint.GetComponentInParent<Animator>();
        instance.PlayAnimation(anim);

        StartCoroutine(instance.ResetAnimator(anim));
        StartCoroutine(instance.PlayEffect(spawnPoint));
        instance.Execute(GameManager.Instance.player.gameObject);

        yield return StartCoroutine(instance.ResetAnimator(anim));

        int slotIndex = GetSlotIndexBySkillId(instance.skillId);
        if (slotIndex >= 0 && skillUI != null)
            skillUI.HideSkillSetting(slotIndex, instance.GetCooldown());
    }

    private void UpdateCooldown(SkillInstance instance)
    {
        nextAvailableTimes[instance.skillId] = Time.time + instance.GetCooldown();
    }

    private bool IsEnoughCost(SkillInstance instance)
    {
        int cost = instance.GetMpCost();

        var playerStatus = GameManager.Instance.player.GetComponent<PlayerStatus>();
        if (playerStatus == null) return false;

        if (playerStatus.stats[StatType.CurrentMP] >= cost)
        {
            playerStatus.stats[StatType.CurrentMP] -= cost;
            return true;
        }

        return false;
    }

    public bool IsMemorySkill(SkillInstance instance)
    {
        return currentMemoryPiece != null &&
               currentMemoryPiece.skillItem != null &&
               currentMemoryPiece.skillItem.memoryPieceId == instance.skillId;
    }

    private int GetSlotIndexBySkillId(int skillId)
    {
        var sc = WeaponManager.Instance.skillController;

        if (currentMemoryPiece != null && currentMemoryPiece.skillItem != null && currentMemoryPiece.skillItem.memoryPieceId == skillId)
            return 0;
        if (sc.skill01 != null && sc.skill01.skillId == skillId) return 2;
        if (sc.skill02 != null && sc.skill02.skillId == skillId) return 3;

        return -1;
    }

    public SkillInstance GetSkillInstance(int skillId)
    {
        skillInstances.TryGetValue(skillId, out var instance);
        return instance;
    }
}
