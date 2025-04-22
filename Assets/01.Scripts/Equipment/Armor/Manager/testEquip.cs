using UnityEngine;

public class ArmorTestEquip : MonoBehaviour
{
    [SerializeField] private int testArmorID = 400; // Resources/Armor 폴더에 있는 SO 이름

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            TryEquipArmor();
        }
    }

    private void TryEquipArmor()
    {
        if (DataManager.Instance.armorSODic.TryGetValue(testArmorID, out var armorSO))
        {
            EquipmentManager.Instance.EquipArmor(armorSO);
        }
        else
        {
            Debug.LogWarning($"[Test] ArmorSO ID : '{testArmorID}'을 찾을 수 없습니다.");
        }
    }
}
