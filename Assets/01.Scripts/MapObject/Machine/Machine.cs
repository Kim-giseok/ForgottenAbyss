using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    [SerializeField] protected Animator machineAnim;
    protected bool isActivated = false;

    public virtual void Active(LaberBase rootlaber)
    {
        if (isActivated) return;
        machineAnim.SetFloat("Active", 1);
        isActivated = true;

        Debug.Log(name + " activated");
    }

    public virtual void UnActive()
    {
        if (!isActivated) return;
        machineAnim.SetFloat("Active", -1);
        isActivated = false;
    }

    protected void ActionAfterAnimation(Action action, float additionalWaitTime = 0f)
    {
        StartCoroutine(WaitAniTime(action, additionalWaitTime));
    }

    IEnumerator WaitAniTime(Action action, float additionalWaitTime = 0f)
    {
        yield return null;
        yield return new WaitForSeconds(machineAnim.GetCurrentAnimatorStateInfo(0).length);

        yield return new WaitForSeconds(additionalWaitTime);

        action.Invoke();
    }
}
