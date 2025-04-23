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
            { KeyCode.Escape, HandleEscapeKey }
            //{ KeyCode.Tab, () => UIManager.Instance?.SwapWeapons() }
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
        if (UIManager.Instance.shopUI.activeSelf)
        {
            // 상점이 열린 상태면 상점만 닫음
            UIManager.Instance.shopUI.SetActive(false);
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                PlayerInput playerInput = player.GetComponent<PlayerInput>();
                if (playerInput != null) playerInput.enabled = true;
            }

        }
        else
        {
            //상점이 안열려 있으면 설정창 열기
            UIManager.Instance?.ToggleSettings();
        }
    }
}
