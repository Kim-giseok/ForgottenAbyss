using System;
using Unity.VisualScripting;
using UnityEngine;

public class BossMemoryItemEvent: MonoBehaviour
{
    private Action events;
    public void SetEvent(Action newEvent) => events += newEvent;
    private void OnDisable()
    {
        events?.Invoke();
        Destroy(this);
    }
}