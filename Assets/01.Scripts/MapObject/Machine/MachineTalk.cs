using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineTalk : MachineLoop
{
    [SerializeField] NpcSentence sentence;

    public override void Active(LaberBase rootlaber)
    {
        base.Active(rootlaber);
        if (!isActivated) return;

        GameManager.Instance.PausePlayer();
        DisplaySentence();
    }

    void DisplaySentence()
    {
        sentence.TalkNpc();
    }
}
