using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    private Dictionary<int, float> nextAvailableTimes = new();
    private Dictionary<int, SkillInstance> skillInstances = new();
    private MemoryPieceSO currentMemoryPiece;

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
        skillInstances.Clear();

        var weaponData = DataManager.Instance.GetWeaponData(weaponId);
        if (weaponData == null) return;

        TryAddSkillInstance(weaponData.ComboAttack);
        TryAddSkillInstance(weaponData.RangedAttack);
        TryAddSkillInstance(weaponData.Skill1Id);
        TryAddSkillInstance(weaponData.Skill2Id);
    }

    public void SetMemorySkill(MemoryPieceSO memorySO)
    {
        if (memorySO == null || memorySO.skillItem == null) return;

        int memoryId = memorySO.currentMemoryPieceId;

        if (!skillInstances.ContainsKey(memoryId))
            skillInstances[memoryId] = new SkillInstance(memorySO);
    }

    private void TryAddSkillInstance(int skillId)
    {
        if (DataManager.Instance.HasSkillData(skillId))
        {
            var data = DataManager.Instance.GetSkillData(skillId);
            skillInstances[skillId] = new SkillInstance(data);
        }
    }

    public bool IsSkillEquipped(int skillId)
    {
        bool isMemory = currentMemoryPiece != null &&
                        currentMemoryPiece.skillItem != null &&
                        currentMemoryPiece.skillItem.memoryPieceId == skillId;

        bool isWeaponSkill = skillInstances.ContainsKey(skillId);

        return isMemory || isWeaponSkill;
    }

    private bool IsOnCooldown(int skillId)
    {
        if (nextAvailableTimes.TryGetValue(skillId, out float nextTime) && Time.time < nextTime)
        {
            Debug.Log($"Skill {skillId} is on cooldown. {nextTime - Time.time:F1}s left.");
            return true;
        }
        return false;
    }

    public void TryUseSkill(int skillId, Transform spawnPoint)
    {
        if (!IsSkillEquipped(skillId)) return;
        if (IsOnCooldown(skillId)) return;

        var instance = skillInstances[skillId];
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
        if (slotIndex >= 0)
            skillUI?.HideSkillSetting(slotIndex, instance.GetCooldown());
    }

    private void UpdateCooldown(SkillInstance instance)
    {
        nextAvailableTimes[instance.skillId] = Time.time + instance.GetCooldown();
    }

    public bool IsMemorySkill(int skillId)
    {
        return currentMemoryPiece != null && currentMemoryPiece.skillItem != null &&
               currentMemoryPiece.skillItem.memoryPieceId == skillId;
    }

    private int GetSlotIndexBySkillId(int skillId)
    {
        var sc = WeaponManager.Instance.skillController;
        if (IsMemorySkill(skillId)) return 0;
        if (skillId == sc.combatId) return 1;
        if (skillId == sc.skill01Id) return 2;
        if (skillId == sc.skill02Id) return 3;
        return -1;
    }
    public SkillInstance GetSkillInstance(int skillId)
    {
        skillInstances.TryGetValue(skillId, out var instance);
        return instance;
    }
}
