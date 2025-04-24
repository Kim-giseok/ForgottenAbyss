using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineTalk : MachineLoop
{
    [SerializeField] NpcSentence sentence;

    public override void Active()
    {
        base.Active();
        DisplaySentence();
    }

    void DisplaySentence()
    {
        sentence.TalkNpc();
    }
}
