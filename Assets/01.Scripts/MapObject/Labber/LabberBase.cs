using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabberBase : MonoBehaviour
{
    IMachine targetMachine;

    public virtual void SwitchMachine()
    {
        targetMachine.Active();
    }
}
