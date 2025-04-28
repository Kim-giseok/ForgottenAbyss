using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shop : MonoBehaviour, IInteractable
{
    public ShopUI shopUI => UIManager.Instance.shopUI;

    public void ActiveInteraction()
    {
        GameManager.Instance.PausePlayer();
        UIManager.Instance.OffGuidUI();
        shopUI.gameObject.SetActive(true);
    }

    public void ReadyInteraction()
    {

    }
}
