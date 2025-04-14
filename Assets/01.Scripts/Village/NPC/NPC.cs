using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

public class NPC : MonoBehaviour, IInteractable
{
    NpcSentence npcSentence;
    PlayerInput playerInput;

    private void Awake()
    {
        npcSentence = GetComponent<NpcSentence>();
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
    }

    public void ActiveInteraction()
    {
        playerInput.enabled = false;
        npcSentence.TalkNpc();
    }

    public void ReadyInteraction()
    {

    }
}
