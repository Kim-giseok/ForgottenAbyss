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
    public GameObject npcText => UIManager.Instance.npcText; //npc에게 다가가면 활성화 되는 안내문구
    
    private void Awake()
    {
        npcSentence = GetComponent<NpcSentence>();
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
    }

    public void ActiveInteraction()
    {
        //playerInput.enabled = false;
        npcText.SetActive(false);
        npcSentence.TalkNpc();
    }

    public void ReadyInteraction()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            npcText.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            npcText.SetActive(false);
        }
    }
}
