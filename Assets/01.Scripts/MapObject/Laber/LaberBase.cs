using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaberBase : MonoBehaviour
{
    [SerializeField] Machine targetMachine;
    [SerializeField] Animator laberAnim;
    protected bool isSwitched = false;

    protected virtual void SwitchMachine()
    {
        if (isSwitched || targetMachine == null) return;
        laberAnim?.SetFloat("Active", 1);
        targetMachine.Active();
        isSwitched = true;
    }

    protected virtual void DisSwitchMachine()
    {
        if (!isSwitched || targetMachine == null) return;
        laberAnim?.SetFloat("Active", -1);
        targetMachine.UnActive();
        isSwitched = false;
    }
}
