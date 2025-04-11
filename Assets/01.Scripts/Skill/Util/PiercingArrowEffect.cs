using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingArrowEffect : MonoBehaviour
{
    public float length = 12f;
    public float duration = 0.1f;
    public LineRenderer line;

    public void Initialize(Vector3 start, Vector3 direction)
    {
        Vector3 end = start + direction * length;

        transform.position = start;
        line.SetPosition(0, start);
        line.SetPosition(1, end);

        Invoke(nameof(Disable), duration);
    }

    private void OnEnable()
    {
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
