using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineTalk : Machine
{
    [Header("TalkParameter")]
    [SerializeField] NpcSentence sentence;

    public override void Active(LaberBase rootlaber)
    {
        activatedEvent.RemoveListener(DisplaySentence);
        activatedEvent.AddListener(DisplaySentence);

        base.Active(rootlaber);
    }

    void DisplaySentence()
    {
        sentence.TalkNpc();
    }
}
