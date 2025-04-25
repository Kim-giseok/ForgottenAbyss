using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaberBase : MonoBehaviour
{
    [SerializeField] Machine[] targetMachines;
    [SerializeField] Animator laberAnim;
    protected bool isSwitched = false;

    public virtual void SwitchMachine()
    {
        if (isSwitched) return;
        laberAnim?.SetFloat("Active", 1);
        foreach (var targetMachine in targetMachines)
            targetMachine.Active(this);
        isSwitched = true;
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
