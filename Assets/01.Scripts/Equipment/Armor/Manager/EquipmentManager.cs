using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class EquipmentManager : MonoBehaviour
{
    private Dictionary<ArmorSlot, (ArmorSO armor, InventorySlotUI slot)> equippedArmors = new();
    private CharacterStatus playerStatus;

    public event Action<ArmorSO> OnEquipArmor;
    public event Action<ArmorSO> OnUnequipArmor;

    public event Action<MemoryPieceSO> OnEquipMemory;
    public event Action<MemoryPieceSO> OnUnequipMemory;

    private MemoryPieceSO equippedMemorySO;
    private InventorySlotUI equippedMemorySlot;

    public bool isSetBonusApplied = false;

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

        ReApplyArmorStats();

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

    public void EquipArmor(ArmorSO armor, InventorySlotUI slot = null)
    {
        if (equippedArmors.TryGetValue(armor.slot, out var equippedArmor))
        {
            UnequipArmor(armor.slot); // 기존 장비 해제
            Debug.Log($"[EquipmentManager] 기존 {equippedArmor.armor.name} 해제 후 {armor.name} 장착");
        }

        equippedArmors[armor.slot] = (armor, slot);
        ApplyStatBonus(armor);
        RemoveSetBonus(); // 기존 보너스 제거 후
        ApplySetBonus();

        OnEquipArmor?.Invoke(armor);

        string bonusLog = string.Join(", ", armor.statBonuses.Select(b => $"{b.statType} {b.bonusType} +{b.value}"));
        Debug.Log($"[Test] 장착 성공: {armor.name} → {armor.slot}, {bonusLog}");
    }

    public void UnequipArmor(ArmorSlot slot)
    {
        if (equippedArmors.TryGetValue(slot, out var armor))
        {
            RemoveSetBonus();
            RemoveStatBonus(armor.armor);

            equippedArmors.Remove(slot);
            OnUnequipArmor?.Invoke(armor.armor);

            string bonusLog = string.Join(", ", armor.armor.statBonuses.Select(b => $"{b.statType} {b.bonusType} -{b.value}"));
            Debug.Log($"[Test] 장착 해제: {armor.armor.name} → {armor.slot}, {bonusLog}");
        }
    }

    public ArmorSO GetEquippedArmor(ArmorSlot slot)
    {
        equippedArmors.TryGetValue(slot, out var armor);
        return armor.armor;
    }

    public InventorySlotUI GetEquippedArmorSlot(ArmorSlot slot)
    {
        if (equippedArmors.TryGetValue(slot, out var data))
            return data.slot;
        return null;
    }

    //public void UpdateEquippedItems()
    //{
    //    if (equippedArmors != null && equippedArmors.Any())
    //    {
    //        Debug.Log($"[EquipmentManager] UpdateEquippedItems() 실행 - 장착 정보 갱신 중!");

    //        Dictionary<ArmorSlot, (ArmorSO armor, InventorySlotUI slot)> updatedEquippedArmors = new();
    //        InventorySlotUI newEquippedMemorySlot = null;
    //        MemoryPieceSO newEquippedMemoryPieceSo = null;

    //        foreach (var slot in InventoryUIManager.Instance.slots)
    //        {
    //            if (slot.currentItem != null && slot.currentItem.itemType == ItemType.Equip && slot.currentItem is ArmorSO armor)
    //            {
    //                updatedEquippedArmors[armor.slot] = (armor, slot);
    //                Debug.Log($"[EquipmentManager] 장착 정보 갱신 - 슬롯: {armor.slot}, 장착 장비: {armor.name}");
    //            }
    //            else if (slot.currentItem != null && slot.currentItem.itemType == ItemType.Memory && slot.currentItem is MemorySkillItem memorySkill)
    //            {
    //                newEquippedMemoryPieceSo = SystemManager.Instance.dataManager.GetMemoryVisualSOById(memorySkill.memoryPieceId);
    //                newEquippedMemorySlot = slot;
    //                Debug.Log($"[EquipmentManager] 기억 아이템 장착 - {memorySkill.skillName}");
    //            }
    //        }
    //        equippedArmors = updatedEquippedArmors;
    //        equippedMemorySO = newEquippedMemoryPieceSo;
    //        equippedMemorySlot = newEquippedMemorySlot;
    //    }
    //    else
    //    {
    //        Debug.Log($"[EquipmentManager] 장착된 아이템 없음 → UpdateEquippedItems 실행 안 함!");
    //    }
    //}

    public Dictionary<StatType, float> GetTotalArmorStats()
    {
        Dictionary<StatType, float> total = new();

        foreach (var armor in equippedArmors.Values)
        {
            foreach (var bonus in armor.armor.statBonuses)
            {
                if (!total.ContainsKey(bonus.statType))
                    total[bonus.statType] = 0;

                total[bonus.statType] += bonus.value;
            }
        }

        return total;
    }

    private Dictionary<StatType, float> baseStats = new Dictionary<StatType, float>();

    private void ApplyStatBonus(ArmorSO armor)
    {
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            var bonuses = armor.statBonuses.Where(b => b.statType == statType).ToList();
            if (bonuses.Count == 0) continue;

            playerStatus.stats.TryGetValue(statType, out float baseValue);

            float finalValue = StatBonusCalculator.ApplyBonuses(baseValue, bonuses);

            playerStatus.ApplyEquipmentBonus(statType, finalValue - baseValue, armor.armorId);

            Debug.Log($"[ApplyStatBonus] {statType}: +{finalValue}");
        }
    }

    private void RemoveStatBonus(ArmorSO armor)
    {
        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            var bonuses = armor.statBonuses.Where(b => b.statType == statType).ToList();
            if (bonuses.Count == 0) continue;

            playerStatus.stats.TryGetValue(statType, out float baseValue);

            float restoredValue = StatBonusCalculator.RemoveBonuses(baseValue, bonuses);

            playerStatus.RemoveEquipmentBonus(statType, baseValue - restoredValue);

            Debug.Log($"[RemoveStatBonus] {statType}: {baseValue} → {restoredValue}");
        }
    }

    private void ApplySetBonus()
    {
        // 기본적으로 모든 방어구가 장착되어 있어야 한다고 가정
        if (equippedArmors.Count != 4)
            return;

        // 모든 방어구의 세트 이름이 동일한지 체크
        string setName = equippedArmors.First().Value.armor.setName;
        foreach (var armorPair in equippedArmors)
        {
            if (armorPair.Value.armor.setName != setName)
                return; // 하나라도 세트가 다르면 보너스 미적용
        }


        if (!isSetBonusApplied)
        {
            isSetBonusApplied = true;

            if (ArmorSetBonus.SetBonuses.TryGetValue(setName, out ArmorSetBonus bonusData))
            {
                foreach (var bonus in bonusData.Bonuses)
                {
                    playerStatus.ApplySetBonus(bonus.stat, bonus.multiplier);

                    UIManager.Instance.statUI.equippedItemUI.SetBonusText($"{bonusData.SetName} 세트", $"{bonusData.Description}");
                }
            }
 
            Debug.Log($"{setName} 세트 보너스 적용됨");
        }
    }

    private void RemoveSetBonus()
    {
        if (isSetBonusApplied)
        {
            isSetBonusApplied = false;

            // 방어구의 세트 이름 체크
            string setName = equippedArmors.First().Value.armor.setName;

            if (ArmorSetBonus.SetBonuses.TryGetValue(setName, out ArmorSetBonus bonusData))
            {
                foreach (var bonus in bonusData.Bonuses)
                {
                    playerStatus.RemoveSetBonus(bonus.stat, bonus.multiplier);

                    Debug.Log($"{setName} 세트 보너스 제거 → {bonus.stat}");
                }
            }

            UIManager.Instance.statUI.equippedItemUI.ResetBonusText();
        }
    }

    private void ReApplyArmorStats()
    {
        var currentStats = GetTotalArmorStats();

        foreach (var armor in equippedArmors.Values)
        {
            // 이미 적용된 스탯이면 추가하지 않음
            foreach (var bonus in armor.armor.statBonuses)
            {
                if (currentStats.TryGetValue(bonus.statType, out float existingValue) && existingValue >= bonus.value)
                {
                    Debug.Log($"[ReApplyArmorStats] {armor.armor.name}: {bonus.statType} 중복 적용 X");
                    continue;
                }

                ApplyStatBonus(armor.armor);
            }
        }
    }

    public void SaveEquippedArmors()
    {
        var saveData = new SaveEquipData
        {
            equippedArmors = equippedArmors.Select(kvp => new EquippedSlotData
            {
                slot = kvp.Key,
                armorId = kvp.Value.armor.armorId
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
            if (SystemManager.Instance.dataManager.armorSODic.TryGetValue(entry.armorId, out var armor))
            {
                if (armor.slot == entry.slot)
                {
                    // 이미 장착된 아이템인지 확인 후 중복 장착 방지
                    if (equippedArmors.TryGetValue(entry.slot, out var equippedArmor) && equippedArmor.armor == armor)
                    {
                        Debug.Log($"[Load] 이미 장착된 아이템: {armor.name}, 장착 스킵");
                        continue; // 중복 장착 방지
                    }

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
        //SaveEquippedArmors();
    }

    public void EquipMemoryPiece(MemoryPieceSO memorySO, InventorySlotUI slot)
    {

        if (equippedMemorySO != null && equippedMemorySO.currentMemoryPieceId == memorySO.currentMemoryPieceId)
        {
            // 클릭한 슬롯이 이미 장착된 슬롯이라면 토글(해제)
            if (equippedMemorySlot == slot)
            {
                UnequipMemoryPiece();
                return;
            }
            else
            {
                // 다른 슬롯에서 클릭한 경우에는 기존 장착을 해제한 후 진행
                UnequipMemoryPiece();
            }
        }
        else if (equippedMemorySO != null)
        {
            UnequipMemoryPiece();
        }

        equippedMemorySO = memorySO;
        equippedMemorySlot = slot;
        SystemManager.Instance.weaponManager.EquipMemoryPiece(memorySO);
        OnEquipMemory?.Invoke(memorySO);
    }

    public void UnequipMemoryPiece()
    {
        if (equippedMemorySO == null) return;

        var old = equippedMemorySO;
        equippedMemorySO = null;
        equippedMemorySlot = null;
        SystemManager.Instance.weaponManager.UnequipMemoryPiece();
        OnUnequipMemory?.Invoke(old);
    }

    public MemoryPieceSO GetEquippedMemoryPiece()
    {
        return equippedMemorySO;
    }

    public InventorySlotUI GetEquippedMemorySlot()
    {
        return equippedMemorySlot;
    }

    public bool IsArmorEquipped(ArmorSlot slot)
    {
        return equippedArmors.ContainsKey(slot);
    }

    public bool IsMemoryPieceEquipped(int memoryPieceId)
    {
        var curMemory = SystemManager.Instance.weaponManager.GetCurrentMemoryPieceSO();
        if(curMemory == null) return false;
        if (curMemory.currentMemoryPieceId == memoryPieceId) return true;
        else return false;
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

    public bool IsEquipped(ArmorSO armor)
    {
        if (armor == null) return false;

        if (equippedArmors.TryGetValue(armor.slot, out var equipped))
        {
            return equipped.armor == armor;
        }

        return false;
    }

    public bool IsEquipped(MemorySkillItem memory)
    {
        if (memory == null) return false;

        return equippedMemorySO != null && equippedMemorySO.currentMemoryPieceId == memory.memoryPieceId;
    }
}
