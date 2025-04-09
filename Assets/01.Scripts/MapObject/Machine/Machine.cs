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
        machineAnim.SetTrigger("Active");
        isActivated = true;

        Debug.Log(name + " activated");
    }
}
