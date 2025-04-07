using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<WeaponData> weaponDataList;

    private void Awake()
    {
        weaponDataList = DataLoadUtil.LoadJsonData<WeaponData>("Json/WeaponData");
    }
}
