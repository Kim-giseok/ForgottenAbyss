using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class UIInputHandler : MonoBehaviour
{
    private Dictionary<KeyCode, System.Action> keyActions;

    private void Awake()
    {
        // Singleton�� �ٷ� ���
        keyActions = new Dictionary<KeyCode, System.Action>
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
            // ������ ���� ���¸� ������ ����
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
            // statUI�� ���������� statUI�� ����
            UIManager.Instance.statUI.OffStatUI();
        }
        else if (UIManager.Instance.passiveUI != null && UIManager.Instance.passiveUI.passiveUI != null
             && UIManager.Instance.passiveUI.passiveUI.activeSelf)
        {
            // statUI�� ���������� statUI�� ����
            UIManager.Instance.passiveUI.OffPassiveUI();
        }
        else
        {
            //������ �ȿ��� ������ ����â ����
            UIManager.Instance?.ToggleSettings();
        }
    }
}
