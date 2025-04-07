using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaberInteractive : LaberBase, IInteractable
{
    public void ActiveInteraction()
    {
        SwitchMachine();
    }

    public void ReadyInteraction()
    {
    }
}
