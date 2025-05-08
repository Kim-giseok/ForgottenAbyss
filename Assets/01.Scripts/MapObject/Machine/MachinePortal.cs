using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinePortal : Machine, IInteractable
{
    [SerializeField] NpcSentence sentence;

    public void ActiveInteraction()
    {
        if (!isActivated)
        {
            sentence?.TalkNpc();
            return;
        }
        MapSpawnManager.Instance.SpawnNextMap();
    }

    public void ReadyInteraction()
    {
    }
}
