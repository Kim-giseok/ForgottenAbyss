using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shop : MonoBehaviour, IInteractable
{
    public GameObject shopUI => UIManager.Instance.shopUI;
    public GameObject shopText => UIManager.Instance.npcText;
    PlayerInput playerInput;

    private void Awake()
    {
        //shopUI.SetActive(false);
        //shopText.SetActive(false);
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (shopUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickExit();
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

        UIManager.Instance?.inventoryUI?.Close();
    }
}
