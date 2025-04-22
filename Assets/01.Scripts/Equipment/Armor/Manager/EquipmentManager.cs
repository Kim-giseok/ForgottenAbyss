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
        Find();

        LoadEquippedArmors();
        Debug.Log("[Auto] 애플리케이션 실행 → 장비 불러오기");
    }

    private void Find()
    {
        playerStatus = FindObjectOfType<Player>().GetComponent<CharacterStatus>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("[Test] 5번 키 → 장비 불러오기 시도");
            LoadEquippedArmors();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("[Test] 6번 키 → 장비 저장 시도");
            SaveEquippedArmors();
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
        Debug.Log($"[Test] 장착 성공: {armor.name} → {armor.slot}, {armor.statType} +{armor.value}");
    }

    public void UnequipArmor(ArmorSlot slot)
    {
        if (equippedArmors.TryGetValue(slot, out var armor))
        {
            RemoveStatBonus(armor);
            equippedArmors.Remove(slot);
            OnUnequipArmor?.Invoke(armor);
            Debug.Log($"[Test] 장착 해제: {armor.name} → {armor.slot}, {armor.statType} -{armor.value}");
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
            if (!total.ContainsKey(armor.statType))
                total[armor.statType] = 0;
            total[armor.statType] += armor.value;
        }
        return total;
    }

    private void ApplyStatBonus(ArmorSO armor)
    {
        float current = 0;
        playerStatus.stats.TryGetValue(armor.statType, out current);
        playerStatus.SetStat(armor.statType, current + armor.value);  // 단순 + 인데 %로 바꿔줘도 될듯?
    }

    private void RemoveStatBonus(ArmorSO armor)
    {
        float current = 0;
        playerStatus.stats.TryGetValue(armor.statType, out current);
        playerStatus.SetStat(armor.statType, current - armor.value);  // 단순 - 인데 %로 바꿔줘도 될듯?
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
