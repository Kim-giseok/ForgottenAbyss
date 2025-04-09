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
        laberAnim?.SetTrigger("Active");
        targetMachine.Active();
        isSwitched = true;
    }
}
