using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaberDamagerble : LaberBase, IDamagable
{
    [SerializeField] float damageCut;

    public void GetDamage(float damage)
    {
        if (damage >= damageCut)
            SwitchMachine();
    }
}
