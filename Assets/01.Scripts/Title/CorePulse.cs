using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorePulse : MonoBehaviour
{
    public float pulseSpeed = 1f;
    public float minScale = 0.9f, maxScale = 1.1f;

    void Update()
    {
        float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        float s = Mathf.Lerp(minScale, maxScale, t);
        transform.localScale = Vector3.one * s;
    }
}
