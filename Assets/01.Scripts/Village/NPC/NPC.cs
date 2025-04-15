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
    public GameObject npcText; //npc에게 다가가면 활성화 되는 안내문구
    
    private void Awake()
    {
        npcSentence = GetComponent<NpcSentence>();
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        if (npcText != null)
        {
            // RectTransform을 사용하여 UI 요소 위치 설정 (UI 요소인 경우)
            if (npcText.GetComponent<RectTransform>() != null)
            {
                npcText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 350);
            }
            // 일반 Transform을 사용하여 월드 공간의 위치 설정 (UI가 아닌 경우)
            else
            {
                npcText.transform.position = new Vector2(2, -3);
            }
        }
        else
        {
            Debug.LogError("npcText가 할당되지 않았습니다.");
        }
        npcText.SetActive(false);
    }

    public void ActiveInteraction()
    {
        playerInput.enabled = false;
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
