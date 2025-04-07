using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaberBase : MonoBehaviour
{
    [SerializeField] Machine targetMachine;
    [SerializeField] Animator laberAnim;
    bool isSwitched = false;

    public virtual void SwitchMachine()
    {
        laberAnim.SetTrigger("Active");
        targetMachine.Active();
        isSwitched = true;
    }
}
