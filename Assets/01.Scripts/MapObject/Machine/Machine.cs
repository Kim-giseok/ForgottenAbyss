using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    [SerializeField] Animator machineAnim;
    protected bool isActivated = false;

    public virtual void Active()
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
}
