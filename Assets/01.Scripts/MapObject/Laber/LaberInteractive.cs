using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaberInteractive : LaberBase, IInteractable
{
    public bool unswitchable;

    public override void SwitchMachine()
    {
        base.SwitchMachine();
        UIManager.Instance.OffGuidUI();
    }

    public void ActiveInteraction()
    {
        if (!isSwitched)
            SwitchMachine();
        else if (unswitchable)
            DisSwitchMachine();
    }

    public void ReadyInteraction()
    {
    }
}
