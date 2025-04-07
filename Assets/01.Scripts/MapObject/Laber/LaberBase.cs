using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaberBase : MonoBehaviour
{
    [SerializeField] Machine targetMachine;
    [SerializeField] Animator laberAnim;
    bool isSwitched = false;

    protected virtual void SwitchMachine()
    {
        if (isSwitched) return;
        laberAnim?.SetTrigger("Active");
        targetMachine?.Active();
        isSwitched = true;
    }
}
