using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LaberBase : MonoBehaviour
{
    [SerializeField] Machine[] targetMachines;
    [SerializeField] Animator laberAnim;
    protected bool isSwitched = false;

    [Header("Once active parameter")]
    [SerializeField] bool activeOnce;
    [SerializeField] FLAGKEY flagName = FLAGKEY.DEFAULTFLAG;

    bool isAleadyActivated => activeOnce && ActivateFlag.CheckFlag(flagName);

    public virtual void SwitchMachine()
    {
        if (isSwitched || isAleadyActivated) return;

        laberAnim?.SetFloat("Active", 1);
        foreach (var targetMachine in targetMachines)
            targetMachine.Active(this);

        isSwitched = true;
        ActivateFlag.ActiveFlag(flagName);
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
