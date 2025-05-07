using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LaberBase : MonoBehaviour
{
    [SerializeField] Animator laberAnim;
    [SerializeField] Machine[] targetMachines;
    protected bool isSwitched = false;

    [Header("")]
    [SerializeField] FLAGKEY activatableConditionFlag = FLAGKEY.IGNOREFLAG;
    [SerializeField] FLAGKEY activeOnceFlag = FLAGKEY.IGNOREFLAG;

    bool isActivatable => (activatableConditionFlag == FLAGKEY.IGNOREFLAG || ActivateFlag.CheckFlag(activatableConditionFlag)) && !isSwitched;
    bool isAleadyActivated => activeOnceFlag != FLAGKEY.IGNOREFLAG && ActivateFlag.CheckFlag(activeOnceFlag);

    public virtual void SwitchMachine()
    {
        if (!isActivatable || isAleadyActivated) return;

        laberAnim?.SetFloat("Active", 1);
        foreach (var targetMachine in targetMachines)
            targetMachine.Active(this);

        isSwitched = true;
        ActivateFlag.ActiveFlag(activeOnceFlag);
    }

    public virtual void DisSwitchMachine()
    {
        if (!isSwitched) return;
        laberAnim?.SetFloat("Active", -1);
        foreach (var targetMachine in targetMachines)
            targetMachine.UnActive();
        isSwitched = false;
    }
}
