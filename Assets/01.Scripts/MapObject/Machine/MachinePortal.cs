using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinePortal : Machine, IInteractable
{
    public void ActiveInteraction()
    {
        if (!isActivated) return;
        MapSpawnManager.Instance.SpawnRandomMap();
    }

    public void ReadyInteraction()
    {
    }
}
