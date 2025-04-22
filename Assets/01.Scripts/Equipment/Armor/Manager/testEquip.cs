using System.Collections.Generic;
using UnityEngine;

public class ArmorTestEquip : MonoBehaviour
{
    [SerializeField] private List<int> testArmorIDs = new() { 400, 401, 402, 403 };

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            TryEquipArmor();
        }
    }

    private void TryEquipArmor()
    {
        foreach (int id in testArmorIDs)
        {
            if (DataManager.Instance.armorSODic.TryGetValue(id, out var armorSO))
            {
                EquipmentManager.Instance.EquipArmor(armorSO);
                Debug.Log($"[Test] 장착 시도 → {armorSO.name} ({armorSO.slot})");
            }
            else
            {
                Debug.LogWarning($"[Test] ArmorSO ID '{id}'를 찾을 수 없습니다.");
            }
        }
    }
}
