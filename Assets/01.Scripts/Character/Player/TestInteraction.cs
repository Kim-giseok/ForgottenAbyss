using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteraction : MonoBehaviour, IInteractable
{
    public GameObject interactable;
    public void ReadyInteraction()
    {

    }
    public void ActiveInteraction()
    {
        HideFlatform();
    }

    public void HideFlatform()
    {
        Destroy(interactable);
    }
}
