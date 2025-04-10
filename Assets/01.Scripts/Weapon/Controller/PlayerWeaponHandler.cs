using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    public Transform weaponHolder;             // 무기 장착 위치
    public GameObject defaultWeaponPrefab;     // 장착할 기본 무기 프리팹

    private GameObject currentWeapon;

    void Start()
    {
        EquipWeapon(defaultWeaponPrefab);
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (currentWeapon != null)
            Destroy(currentWeapon);

        currentWeapon = Instantiate(weaponPrefab, weaponHolder);
        currentWeapon.transform.localPosition = weaponHolder.localPosition;
        currentWeapon.transform.localRotation = Quaternion.identity;

        WeaponHitBox weaponHitBox = weaponHolder.GetComponent<WeaponHitBox>();
        if (weaponHitBox != null)
        {
            Transform hitboxTransform = currentWeapon.transform.Find("HitBox");
            if (hitboxTransform != null)
            {
                weaponHitBox.Init(hitboxTransform.gameObject);
                Debug.Log("HitBox 연결 완료!");
            }
            else
            {
                Debug.LogWarning("HitBox 오브젝트를 찾을 수 없습니다.");
            }
        }
    }
}
