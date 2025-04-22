using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    private Dictionary<int, float> nextAvailableTimes = new();
    private List<int> currentWeaponSkillIds = new();
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
        var weaponData = DataManager.Instance.GetWeaponData(weaponId);
        if (weaponData == null) return;

        currentWeaponSkillIds.Clear();

        switch (weaponData.Type)
        {
            case WeaponType.Sword:
                currentWeaponSkillIds.Add(weaponData.ComboAttack);
                break;
            case WeaponType.Bow:
                currentWeaponSkillIds.Add(weaponData.RangedAttack);
                break;
        }

        currentWeaponSkillIds.Add(weaponData.Skill1Id);
        currentWeaponSkillIds.Add(weaponData.Skill2Id);
    }

    public void SetMemorySkill(MemoryPieceSO memorySO)
    {
        if (memorySO == null || memorySO.skillItem == null) return;
        currentMemoryPiece = memorySO;
    }

    public MemorySkillItem GetCurrentMemorySkillData() => currentMemoryPiece?.skillItem;

    private int GetSlotIndexBySkillId(int skillId)
    {
        var sc = WeaponManager.Instance.skillController;
        if (skillId == sc.memorySkillItem?.memoryPieceId) return 0;
        if (skillId == sc.combatId) return 1;
        if (skillId == sc.skill01Id) return 2;
        if (skillId == sc.skill02Id) return 3;
        return -1;
    }

    private bool IsMemorySkill(int skillId) => skillId == GetCurrentMemorySkillData()?.memoryPieceId;

    public bool IsSkillEquipped(int skillId) =>
        skillId == GetCurrentMemorySkillData()?.memoryPieceId || currentWeaponSkillIds.Contains(skillId);

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

        if (IsMemorySkill(skillId))
            ExecuteMemorySkill(skillId);
        else
            ExecuteWeaponSkill(skillId, spawnPoint);

        UpdateCooldown(skillId);
    }

    private void ExecuteMemorySkill(int skillId)
    {
        var memoryItem = GetCurrentMemorySkillData();
        memoryItem?.Use();
        skillUI?.HideSkillSetting((int)SkillSlotType.Memory, 5f); // 임시 쿨타임
    }

    private void ExecuteWeaponSkill(int skillId, Transform spawnPoint)
    {
        var skillData = DataManager.Instance.GetSkillData(skillId);
        if (skillData == null) return;

        PlaySkillAnimation(skillData, spawnPoint);
        PlaySkillEffect(skillData, spawnPoint);
        ExecuteSkillLogic(skillData);
        skillUI?.HideSkillSetting(GetSlotIndexBySkillId(skillId), skillData.CoolTime);
    }

    private void PlaySkillAnimation(SkillData skillData, Transform spawnPoint)
    {
        var visual = DataManager.Instance.GetSkillVisualSO(skillData.VisualSOName);
        if (visual == null) return;

        var anim = spawnPoint.GetComponentInParent<Animator>();
        if (anim == null) return;

        anim.SetBool("IsAttacking", true);
        anim.speed = visual.animationSpeed;

        if (!string.IsNullOrEmpty(visual.animationName))
            anim.Play(visual.animationName);

        StartCoroutine(ResetAnimator(anim, visual.resetTime));
    }

    private void PlaySkillEffect(SkillData skillData, Transform spawnPoint)
    {
        var visual = DataManager.Instance.GetSkillVisualSO(skillData.VisualSOName);
        if (visual == null || visual.skillEffectPrefab == null || visual.isTogether) return;

        StartCoroutine(EffectCoroutine(visual, spawnPoint));
    }

    private void ExecuteSkillLogic(SkillData skillData)
    {
        var exec = DataManager.Instance.GetSkillExecutionSO(skillData.ExecutionSOName);
        exec?.Execute(GameManager.Instance.player.gameObject, null, skillData);
    }

    private void UpdateCooldown(int skillId)
    {
        var skillData = DataManager.Instance.GetSkillData(skillId);
        if (skillData != null)
            nextAvailableTimes[skillId] = Time.time + skillData.CoolTime;
        else if (IsMemorySkill(skillId))
            nextAvailableTimes[skillId] = Time.time + 5f; // 임시
    }

    private IEnumerator ResetAnimator(Animator anim, float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.speed = 1.0f;
        anim.SetBool("IsAttacking", false);
    }

    private IEnumerator EffectCoroutine(SkillVisualSO visual, Transform spawnPoint)
    {
        if (visual.effectDelay > 0f)
            yield return new WaitForSeconds(visual.effectDelay);

        Vector3 dir = spawnPoint.right;
        Vector3 pos = spawnPoint.position + dir * visual.effectXOffset + Vector3.up * visual.effectYOffset;

        var effect = EffectPool.Instance.SpawnEffect(visual.effectKey, pos, spawnPoint.rotation);

        switch (WeaponManager.Instance.GetCurrentWeaponData().Type)
        {
            case WeaponType.Bow:
                effect.GetComponent<PiercingArrowEffect>()?.Initialize(pos, dir);
                break;
            case WeaponType.Sword:
                effect.GetComponent<DashTrailEffect>()?.Initialize(pos, dir);
                break;
        }
    }
}
