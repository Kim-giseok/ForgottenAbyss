using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineLoop : Machine
{
    [SerializeField] LaberBase rootLaber;

    public override void Active()
    {
        base.Active();
        StartCoroutine(WaitAniTime(UnActive));
    }

    public override void UnActive()
    {
        base.UnActive();
        StartCoroutine(WaitAniTime(rootLaber.DisSwitchMachine));
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
