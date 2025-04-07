using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{
    protected bool isActivated = false;

    public virtual void Active()
    {
        if (isActivated) return;
        isActivated = true;
    }
}
