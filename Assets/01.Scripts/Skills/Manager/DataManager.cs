using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public SkillDataList skillDataList;
    public Dictionary<string, SkillDataSO> skillSODic = new Dictionary<string, SkillDataSO>();
    public List<SkillDataSO> skillSOList;

    public WeaponDataList weaponDataList;
    public Dictionary<string, WeaponDataSO> weaponSODic = new Dictionary<string, WeaponDataSO>();
    public List<WeaponDataSO> weaponSOList;

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
    }

    private void LoadSkillData()
    {
        TextAsset json = Resources.Load<TextAsset>("Json/SkillData");
        skillDataList = JsonUtility.FromJson<SkillDataList>(json.text);
    }

    private void InitSkillSO()
    {
        foreach (SkillDataSO so in skillSOList)
        {
            if (!skillSODic.ContainsKey(so.name))
                skillSODic.Add(so.name, so);
        }
    }

    public SkillData GetSkillData(int id)
    {
        return skillDataList.Skills.Find(x => x.Id == id);
    }

    public SkillDataSO GetSkillSO(string name)
    {
        return skillSODic[name];
    }

    private void LoadWeaponData()
    {
        TextAsset json = Resources.Load<TextAsset>("Json/WeaponData");
        weaponDataList = JsonUtility.FromJson<WeaponDataList>(json.text);
    }

    private void InitWeaponSO()
    {
        foreach (WeaponDataSO so in weaponSOList)
        {
            if (!weaponSODic.ContainsKey(so.name))
                weaponSODic.Add(so.name, so);
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
}
