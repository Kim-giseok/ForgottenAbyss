using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shop : MonoBehaviour, IInteractable
{
    public GameObject shopUI => UIManager.Instance.shopUI;

    private void Update()
    {
        if (shopUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickExit();
        }
    }

    public void ActiveInteraction()
    {
        GameManager.Instance.PausePlayer();
        UIManager.Instance.OffGuidUI();
        shopUI.SetActive(true);
    }

    public void ReadyInteraction()
    {

    }

    public void OnClickExit()
    {
        shopUI.SetActive(false);
        GameManager.Instance.ActionPlayer();

        UIManager.Instance?.CloseInventory();
    }
}
