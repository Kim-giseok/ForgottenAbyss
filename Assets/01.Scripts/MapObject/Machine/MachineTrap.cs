using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineTrap : Machine
{
    [SerializeField] int atk;
    [SerializeField] LayerMask attackLayer;

    public override void Active()
    {
        base.Active();
        StartCoroutine(WaitAniTime(UnActive));
    }

    public override void UnActive()
    {
        base.UnActive();
    }

    IEnumerator WaitAniTime(Action action)
    {
        float length = machineAnim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);
        length = machineAnim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        action.Invoke();
    }
}
