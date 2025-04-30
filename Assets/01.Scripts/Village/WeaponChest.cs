using UnityEngine;
using System.Collections;

public class WeaponChest : MonoBehaviour
{
    public void GiveWeapon()
    {
        // 대사 끝났으니 무기 지급
        SystemManager.Instance.weaponManager.EquipDefaultWeapons();
        Debug.Log("무기 지급 완료!");
    }
}
