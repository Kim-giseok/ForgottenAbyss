using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineLoop : Machine
{
    [SerializeField] LaberBase rootLaber;
    [SerializeField] float waitTime;

    public override void Active()
    {
        base.Active();
        StartCoroutine(WaitAniTime(UnActive, waitTime));
    }

    public override void UnActive()
    {
        base.UnActive();
        StartCoroutine(WaitAniTime(rootLaber.DisSwitchMachine, waitTime));
    }

    IEnumerator WaitAniTime(Action action, float additionalWaitTime = 0f)
    {
        for (int i = 0; i < 2; i++)
            yield return new WaitForSeconds(machineAnim.GetCurrentAnimatorStateInfo(0).length);

        yield return new WaitForSeconds(additionalWaitTime);

        action.Invoke();
    }
}
