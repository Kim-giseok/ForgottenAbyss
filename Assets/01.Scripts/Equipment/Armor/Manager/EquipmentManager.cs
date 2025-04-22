using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class EquipmentManager : Singleton<EquipmentManager>
{
    private Dictionary<ArmorSlot, ArmorSO> equippedArmors = new();
    private CharacterStatus playerStatus;

    public event Action<ArmorSO> OnEquipArmor;
    public event Action<ArmorSO> OnUnequipArmor;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => FindObjectOfType<Player>() != null);

        Find();

        yield return new WaitUntil(() => playerStatus.stats != null && playerStatus.stats.Count > 0);

        LoadEquippedArmors();
        Debug.Log("[Start] 플레이어 초기화 후 장비 장착 완료");
    }

    private void Find()
    {
        playerStatus = FindObjectOfType<Player>().GetComponent<CharacterStatus>();
    }

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
        StartCoroutine(DelayedPlayerFindAndApply());
    }

    private IEnumerator DelayedPlayerFindAndApply()
    {
        yield return new WaitUntil(() => FindObjectOfType<Player>() != null);

        Find();

        yield return new WaitUntil(() => playerStatus.stats != null && playerStatus.stats.Count > 0);

        ReapplyArmorStats();

        Debug.Log($"[SceneLoaded] {SceneManager.GetActiveScene().name} → 플레이어 찾기 및 장비 스탯 재적용 완료");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("[Test] 6번 키 → 장비 저장 데이터 초기화");
            ClearEquippedArmorData();
        }
    }

    public void EquipArmor(ArmorSO armor)
    {
        if (equippedArmors.TryGetValue(armor.slot, out var equippedArmor))
        {
            if (equippedArmor == armor)
            {
                UnequipArmor(armor.slot);
                Debug.Log($"[Test] 동일한 장비 재장착 → 해제됨: {armor.name}");
                return;
            }
            else
            {
                UnequipArmor(armor.slot); // 기존 장비 해제
            }
        }

        equippedArmors[armor.slot] = armor;
        ApplyStatBonus(armor);
        OnEquipArmor?.Invoke(armor);

        string bonusLog = string.Join(", ", armor.statBonuses.Select(b => $"{b.statType} {b.bonusType} +{b.value}"));
        Debug.Log($"[Test] 장착 성공: {armor.name} → {armor.slot}, {bonusLog}");
    }

    public void UnequipArmor(ArmorSlot slot)
    {
        if (equippedArmors.TryGetValue(slot, out var armor))
        {
            RemoveStatBonus(armor);
            equippedArmors.Remove(slot);
            OnUnequipArmor?.Invoke(armor);

            string bonusLog = string.Join(", ", armor.statBonuses.Select(b => $"{b.statType} {b.bonusType} -{b.value}"));
            Debug.Log($"[Test] 장착 해제: {armor.name} → {armor.slot}, {bonusLog}");
        }  
    }

    public ArmorSO GetEquippedArmor(ArmorSlot slot)
    {
        equippedArmors.TryGetValue(slot, out var armor);
        return armor;
    }

    public Dictionary<StatType, float> GetTotalArmorStats()
    {
        Dictionary<StatType, float> total = new();

        foreach (var armor in equippedArmors.Values)
        {
            foreach (var bonus in armor.statBonuses)
            {
                if (!total.ContainsKey(bonus.statType))
                    total[bonus.statType] = 0;

                total[bonus.statType] += bonus.value;
            }
        }

        return total;
    }

    private void ApplyStatBonus(ArmorSO armor)
    {
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            var bonuses = armor.statBonuses.Where(b => b.statType == statType).ToList();
            if (bonuses.Count == 0) continue;

            playerStatus.stats.TryGetValue(statType, out float baseValue);
            float finalValue = StatBonusCalculator.ApplyBonuses(baseValue, bonuses);

            playerStatus.SetStat(statType, finalValue);
            Debug.Log($"[ApplyStatBonus] {statType}: {baseValue} → {finalValue}");
        }
    }

    private void RemoveStatBonus(ArmorSO armor)
    {
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            var bonuses = armor.statBonuses.Where(b => b.statType == statType).ToList();
            if (bonuses.Count == 0) continue;

            playerStatus.stats.TryGetValue(statType, out float currentValue);
            float restoredValue = StatBonusCalculator.RemoveBonuses(currentValue, bonuses);

            playerStatus.SetStat(statType, restoredValue);
            Debug.Log($"[RemoveStatBonus] {statType}: {currentValue} → {restoredValue}");
        }
    }

    private void ReapplyArmorStats()
    {
        foreach (var armor in equippedArmors.Values)
        {
            ApplyStatBonus(armor);
        }
    }

    public void SaveEquippedArmors()
    {
        var saveData = new SaveEquipData
        {
            equippedArmors = equippedArmors.Select(kvp => new EquippedSlotData
            {
                slot = kvp.Key,
                armorId = kvp.Value.armorId
            }).ToList()
        };

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("EquippedArmors", json);
        PlayerPrefs.Save();
    }

    public void LoadEquippedArmors()
    {
        string json = PlayerPrefs.GetString("EquippedArmors", null);
        if (string.IsNullOrEmpty(json)) return;

        SaveEquipData data = JsonUtility.FromJson<SaveEquipData>(json);

        foreach (var entry in data.equippedArmors)
        {
            if (DataManager.Instance.armorSODic.TryGetValue(entry.armorId, out var armor))
            {
                // 슬롯 정보는 armorSO에도 있지만, 복구 신뢰도를 높이기 위해 슬롯을 재검
                if (armor.slot == entry.slot)
                {
                    EquipArmor(armor);
                }
                else
                {
                    Debug.LogWarning($"슬롯 불일치: ID {entry.armorId}가 {entry.slot}에 저장되어 있으나, 실제 SO의 슬롯은 {armor.slot}");
                    EquipArmor(armor); // 강제 장착
                }
            }
            else
            {
                Debug.LogWarning($"장비 ID {entry.armorId}에 해당하는 ArmorSO를 찾을 수 없습니다.");
            }
        }
    }

    public void ClearEquippedArmorData()
    {
        PlayerPrefs.DeleteKey("EquippedArmors");
        PlayerPrefs.Save();
        Debug.Log("[Clear] 장비 저장 데이터 초기화 완료");
    }

    private void OnApplicationQuit()
    {
        Debug.Log("[Auto] 애플리케이션 종료 → 장비 저장");
        SaveEquippedArmors();
    }

    [Serializable]
    private class SaveEquipData
    {
        public List<EquippedSlotData> equippedArmors;
    }

    [Serializable]
    private class EquippedSlotData
    {
        public ArmorSlot slot;
        public int armorId;
    }
}
