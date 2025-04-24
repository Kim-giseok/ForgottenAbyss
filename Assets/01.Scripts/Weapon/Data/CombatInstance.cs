using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatInstance
{
    public int id;
    public WeaponType weaponType;
    public ComboAttackSO comboSO;
    public RangedAttackSO rangedSO;
    public Sprite icon;

    private ComboAttack comboAttack;
    private RangedAttack rangedAttack;

    public CombatInstance(ComboAttack combo, ComboAttackSO comboSO)
    {
        weaponType = WeaponType.Sword;
        comboAttack = combo;
        this.comboSO = comboSO;
        id = comboSO.id;
        icon = comboSO.icon;
    }

    public CombatInstance(RangedAttack ranged, RangedAttackSO rangedSO)
    {
        weaponType = WeaponType.Bow;
        rangedAttack  = ranged; 
        this.rangedSO = rangedSO;
        id = rangedSO.id;
        icon = rangedSO.icon;
    }

    public void Execute()
    {
        switch (weaponType)
        {
            case WeaponType.Sword:
                //comboAttack.SetComboData(comboSO);
                comboAttack.HandleAttackInput();
                break;
            case WeaponType.Bow:
                //rangedAttack.SetRangedAttackData(rangedSO);
                rangedAttack.HandleAttackInput();
                break;
        }
    }

    public Sprite GetIcon() => icon;
}
