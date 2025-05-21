using System;
using UnityEngine;

public class MudTile: MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out ControllerPlayer controller)) return;
        controller.rigid.drag = 10f;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent(out ControllerPlayer controller)) return;
        controller.rigid.drag = 0f;
    }
}