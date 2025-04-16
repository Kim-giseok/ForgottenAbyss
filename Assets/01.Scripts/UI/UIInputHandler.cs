using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIInputHandler : MonoBehaviour
{
    private Dictionary<KeyCode, Action> keyActions;

    private void Awake()
    {
        // Singleton을 바로 사용
        keyActions = new Dictionary<KeyCode, Action>
        {
            { KeyCode.I, () => UIManager.Instance?.ToggleInventory() },
            { KeyCode.Escape, () => UIManager.Instance?.ToggleSettings() },
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
}
