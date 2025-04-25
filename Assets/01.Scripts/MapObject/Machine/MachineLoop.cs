using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineLoop : Machine
{
    [Header("Loop parameter")]
    [SerializeField] float waitTime;
    LaberBase rootLaber;

    public override void Active(LaberBase rootlaber)
    {
        rootLaber = rootlaber;
        base.Active(rootlaber);
        ActionAfterAnimation(UnActive, waitTime);
    }

    public override void UnActive()
    {
        base.UnActive();
        ActionAfterAnimation(rootLaber.DisSwitchMachine);
    }
}
