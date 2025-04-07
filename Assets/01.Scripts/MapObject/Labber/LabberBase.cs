using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabberBase : MonoBehaviour
{
    public Machine targetMachine;

    public virtual void SwitchMachine()
    {
        targetMachine.Active();
    }
}
