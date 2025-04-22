using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : Singleton<EquipmentManager>
{
    private Dictionary<ArmorSlot, ArmorSO> equippedArmors = new();
    private CharacterStatus playerStatus;

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

        Find();
    }

    private void Find()
    {
        playerStatus = FindObjectOfType<Player>().GetComponent<CharacterStatus>();
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
        Debug.Log($"[Test] 장착 성공: {armor.name} → {armor.slot}, {armor.statType} +{armor.value}");
    }

    public void UnequipArmor(ArmorSlot slot)
    {
        if (equippedArmors.TryGetValue(slot, out var armor))
        {
            RemoveStatBonus(armor);
            equippedArmors.Remove(slot);
        }
        Debug.Log($"[Test] 장착 해제: {armor.name} → {armor.slot}, {armor.statType} -{armor.value}");
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
}
