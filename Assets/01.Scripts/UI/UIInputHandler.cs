using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class UIInputHandler : MonoBehaviour
{
    private Dictionary<KeyCode, Action> keyActions;

    private void Awake()
    {
        // Singleton을 바로 사용
        keyActions = new Dictionary<KeyCode, Action>
        {
            { KeyCode.I, () => UIManager.Instance?.ToggleInventory() },
            { KeyCode.Escape, HandleEscapeKey },
            //{ KeyCode.Tab, () => UIManager.Instance?.SwapWeapons() }
            { KeyCode.V, () => UIManager.Instance?.OnStatUI() },
            { KeyCode.K, () => UIManager.Instance?.OnPassiveUI() }
        };
    }

    private void Update()
    {
        foreach (var entry in keyActions)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                entry.Value?.Invoke();
            }
        }
    }

    private void HandleEscapeKey()
    {
        if (UIManager.Instance.shopUI.gameObject.activeSelf)
        {
            // 상점이 열린 상태면 상점만 닫음
            UIManager.Instance.shopUI.gameObject.SetActive(false);
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                PlayerInput playerInput = player.GetComponent<PlayerInput>();
                if (playerInput != null) playerInput.enabled = true;
            }
        }
        else if (UIManager.Instance.statUI != null && UIManager.Instance.statUI.statusUI != null
             && UIManager.Instance.statUI.statusUI.activeSelf)
        {
            // statUI가 열려있으면 statUI만 닫음
            UIManager.Instance.statUI.OffStatUI();
        }
        else if (UIManager.Instance.passiveUI != null && UIManager.Instance.passiveUI.passiveUI != null
             && UIManager.Instance.passiveUI.passiveUI.activeSelf)
        {
            // statUI가 열려있으면 statUI만 닫음
            UIManager.Instance.passiveUI.OffPassiveUI();
        }
        else
        {
            //상점이 안열려 있으면 설정창 열기
            UIManager.Instance?.ToggleSettings();
        }
    }
}
