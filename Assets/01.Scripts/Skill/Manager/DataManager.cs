using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public SkillDataList skillDataList;
    public WeaponDataList weaponDataList;
    public MemoryPieceDataList memoryPieceDataList;

    public Dictionary<string, SkillVisualSO> skillVisualSODic = new Dictionary<string, SkillVisualSO>();
    public List<SkillVisualSO> skillVisualSOList;

    public Dictionary<string, SkillExecutionSO> skillExecutionSODic = new Dictionary<string, SkillExecutionSO>();
    public List<SkillExecutionSO> skillExecutionSOList;

    public Dictionary<string, WeaponDataSO> weaponSODic = new Dictionary<string, WeaponDataSO>();
    public List<WeaponDataSO> weaponSOList;

    public Dictionary<string, MemoryPieceSO> memoryVisualSODic = new Dictionary<string, MemoryPieceSO>();
    public List<MemoryPieceSO> memoryVisualSOList;

    private void Awake()
    {
        LoadAllData();
    }

    private void LoadAllData()
    {
        LoadSkillData();
        InitSkillSO();

        LoadWeaponData();
        InitWeaponSO();

        LoadMemoryPieceData();
        InitMemoryPieceSO();
    }

    private void LoadSkillData()
    {
        TextAsset json = Resources.Load<TextAsset>("Json/SkillData");
        skillDataList = JsonUtility.FromJson<SkillDataList>(json.text);
    }

    private void InitSkillSO()
    {
        var allVisualSOs = Resources.LoadAll<SkillVisualSO>("Skill/Visual");
        skillVisualSOList = new List<SkillVisualSO>(allVisualSOs);

        foreach (var so in allVisualSOs)
        {
            if (!skillVisualSODic.ContainsKey(so.name))
            {
                skillVisualSODic.Add(so.name, so);
            }
        }

        var allExecutionSOs = Resources.LoadAll<SkillExecutionSO>("Skill/Execution");
        skillExecutionSOList = new List<SkillExecutionSO>(allExecutionSOs);

        foreach (var so in allExecutionSOs)
        {
            if (!skillExecutionSODic.ContainsKey(so.name))
            {
                skillExecutionSODic.Add(so.name, so);
            }
        }
    }

    public SkillData GetSkillData(int id)
    {
        var data = skillDataList.Skills.Find(x => x.Id == id);
        if (data == null)
        {
            Debug.LogWarning($"SkillData with ID {id} not found.");
        }
        return data;
    }

    public SkillVisualSO GetSkillVisualSO(string name)
    {
        if (skillVisualSODic.TryGetValue(name, out var so))
            return so;

        Debug.LogWarning($"SkillVisualSO with name {name} not found.");
        return null;
    }

    public SkillExecutionSO GetSkillExecutionSO(string name)
    {
        if (skillExecutionSODic.TryGetValue(name, out var so))
            return so;

        Debug.LogWarning($"SkillExecutionSO with name {name} not found.");
        return null;
    }

    private void LoadWeaponData()
    {
        TextAsset json = Resources.Load<TextAsset>("Json/WeaponData");
        weaponDataList = JsonUtility.FromJson<WeaponDataList>(json.text);
    }

    private void InitWeaponSO()
    {
        var allWeaponSOs = Resources.LoadAll<WeaponDataSO>("Weapon");
        weaponSOList = new List<WeaponDataSO>(allWeaponSOs);

        foreach (WeaponDataSO so in allWeaponSOs)
        {
            if (!weaponSODic.ContainsKey(so.name))
            {
                weaponSODic.Add(so.name, so);
            }

            WeaponData matchedData = weaponDataList.Weapons.Find(w => w.Id == so.currentWeaponId);
            if (matchedData != null)
            {
                var basicData = GetSkillData(matchedData.basicAttackID);
                var skill01Data = GetSkillData(matchedData.Skill1Id);
                var skill02Data = GetSkillData(matchedData.Skill2Id);

                so.basicAttack = GetSkillVisualSO(basicData?.VisualSOName);
                so.skill01SO = GetSkillVisualSO(skill01Data?.VisualSOName);
                so.skill02SO = GetSkillVisualSO(skill02Data?.VisualSOName);
            }
            else
            {
                Debug.LogWarning($"WeaponData not found for WeaponDataSO '{so.name}' with ID {so.currentWeaponId}");
            }
        }
    }

    public WeaponData GetWeaponData(int id)
    {
        return weaponDataList.Weapons.Find(x => x.Id == id);
    }

    public WeaponDataSO GetWeaponSO(string name)
    {
        return weaponSODic[name];
    }

    private void LoadMemoryPieceData()
    {
        TextAsset json = Resources.Load<TextAsset>("Json/MemoryPieceData");
        memoryPieceDataList = JsonUtility.FromJson<MemoryPieceDataList>(json.text);
    }

    private void InitMemoryPieceSO()
    {
        var allMemoryVisualSOs = Resources.LoadAll<MemoryPieceSO>("Weapon");
        memoryVisualSOList = new List<MemoryPieceSO>(allMemoryVisualSOs);

        foreach (var so in allMemoryVisualSOs)
        {
            if (!memoryVisualSODic.ContainsKey(so.name))
            {
                memoryVisualSODic.Add(so.name, so);
            }

            MemoryPieceData matchedData = memoryPieceDataList.MemoryPieces.Find(m => m.Id == so.currentMemoryPieceId);
            if (matchedData != null)
            {
                SkillData skillData = GetSkillData(matchedData.SkillId);
                if (skillData != null)
                {
                    so.memorySkillVisualSO = GetSkillVisualSO(skillData.VisualSOName);
                }
            }
            else
            {
                Debug.LogWarning($"MemoryPieceData not found for MemoryPieceSO '{so.name}' with ID {so.currentMemoryPieceId}");
            }
        }
    }

    public MemoryPieceData GetMemoryPieceData(int id)
    {
        return memoryPieceDataList.MemoryPieces.Find(x => x.Id == id);
    }

    public MemoryPieceSO GetMemoryVisualSO(string name)
    {
        if (memoryVisualSODic.TryGetValue(name, out var so))
            return so;

        Debug.LogWarning($"MemoryVisualSO with name {name} not found.");
        return null;
    }
}
