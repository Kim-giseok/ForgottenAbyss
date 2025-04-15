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
        ActionAfterAnimation(UnActive, waitTime);
    }

    public override void UnActive()
    {
        base.UnActive();
        ActionAfterAnimation(rootLaber.DisSwitchMachine);
    }
}
