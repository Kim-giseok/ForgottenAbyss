using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugActive : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log($"{gameObject.name} - Awake (DebugActive)");
    }

    private void OnDisable()
    {
        Debug.LogWarning($"{gameObject.name} - DISABLED! by callstack:\n{System.Environment.StackTrace}");
    }
}
