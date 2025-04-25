using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaberInteractive : LaberBase, IInteractable
{
    public override void SwitchMachine()
    {
        base.SwitchMachine();
        UIManager.Instance.OffGuidUI();
    }

    public void ActiveInteraction()
    {
        SwitchMachine();
    }

    public void ReadyInteraction()
    {
    }
}
