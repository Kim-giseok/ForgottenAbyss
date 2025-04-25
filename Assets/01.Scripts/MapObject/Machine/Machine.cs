using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FLAGKEY
{
    DEFAULTFLAG,
    INTRO_DIALOGUE
}

public static class ActivateFlag
{
    static Dictionary<FLAGKEY, bool> flags = new();

    public static void ActiveFlag(FLAGKEY key)
    {
        flags[key] = true;
    }

    public static bool CheckFlag(FLAGKEY key)
    {
        return flags.ContainsKey(key) && flags[key];
    }
}

public class Machine : MonoBehaviour
{
    [SerializeField] protected Animator machineAnim;
    [SerializeField] protected bool isActivated = false;

    [Header("Once active parameter")]
    [SerializeField] bool activeOnce;
    [SerializeField] FLAGKEY flagName = FLAGKEY.DEFAULTFLAG;

    bool isAleadyActivated => activeOnce && ActivateFlag.CheckFlag(flagName);

    public virtual void Active(LaberBase rootlaber)
    {
        if (isActivated || isAleadyActivated) return;
        machineAnim.SetFloat("Active", 1);
        isActivated = true;

        ActivateFlag.ActiveFlag(flagName);
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
