using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shop : MonoBehaviour, IInteractable
{
    public GameObject shopUI;
    public GameObject shopText;
    PlayerInput playerInput;

    private void Awake()
    {
        shopUI.SetActive(false);
        shopText.SetActive(false);
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();

        if (shopText != null)
        {
            // RectTransform을 사용하여 UI 요소 위치 설정 (UI 요소인 경우)
            if (shopText.GetComponent<RectTransform>() != null)
            {
                shopText.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, 350);
            }
           
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.CompareTag("Player"))
        {
            shopText.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            shopText.SetActive(false);
        }
    }

    public void ActiveInteraction()
    {
        playerInput.enabled = false;
        shopText.SetActive(false);
        shopUI.SetActive(true);
    }

    public void ReadyInteraction()
    {

    }

    public void OnClickExit()
    {
        shopUI.SetActive(false);
        playerInput.enabled = true;
    }
}
