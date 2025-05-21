using System;
using Unity.VisualScripting;
using UnityEngine;

public class AgisMemoryItemEvent: MonoBehaviour
{
    private Action events;
    public void SetEvent(Action newEvent) => events += newEvent;
    private void OnDisable() => events?.Invoke();
}