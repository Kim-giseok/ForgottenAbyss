using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineTrap : Machine
{
    [Header("TrapParameter")]
    [SerializeField] float atk;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<IDamagable>(out IDamagable damaged)) return;

        damaged.GetDamage(atk);
    }
}
