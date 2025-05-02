using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Machine : MonoBehaviour
{
    [SerializeField] protected Animator machineAnim;
    [SerializeField] protected bool isActivated = false;

    [SerializeField] protected UnityEvent activatedEvent;
    [SerializeField] protected UnityEvent unactivatedEvent;

    [Header("Loop parameter")]
    [SerializeField] protected bool isLoop;
    [SerializeField] protected float waitTime;
    protected LaberBase rootLaber;

    public virtual void Active(LaberBase rootlaber)
    {
        rootLaber = rootlaber;
        if (isActivated) return;
        machineAnim.SetFloat("Active", 1);
        isActivated = true;

        ActionAfterAnimation(activatedEvent.Invoke);
        Debug.Log(name + " activated");
        if (isLoop) ActionAfterAnimation(UnActive, waitTime);
    }

    public virtual void UnActive()
    {
        if (!isActivated) return;
        machineAnim.SetFloat("Active", -1);
        isActivated = false;

        ActionAfterAnimation(unactivatedEvent.Invoke);
        if (isLoop) ActionAfterAnimation(rootLaber.DisSwitchMachine);
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
