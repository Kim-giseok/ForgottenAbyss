using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FLAGKEY
{
    DEFAULTFLAG,
    INTRO_DIALOGUE,
    FIRST_BATTLE_TUTORIAL,
    WEAPON_SWAP_TUTORIAL
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
