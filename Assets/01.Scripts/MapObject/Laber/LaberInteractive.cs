using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaberInteractive : LaberBase, IInteractable
{
    protected override void SwitchMachine()
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isSwitched)
            UIManager.Instance.OnGuidUI(this);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            UIManager.Instance.OffGuidUI();        
    }
}
