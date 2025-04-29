using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightBreath: MonoBehaviour
{
    private Light2D light;
    
    public float min;
    public float max;
    public float speed;

    private void Awake()
    {
        light = GetComponent<Light2D>();
    }

    private void Update()
    {
        if (!light) return;

        float sin = Mathf.Sin(Time.time * speed) * 0.5f + 0.5f;
        light.pointLightOuterRadius = Mathf.Lerp(min, max, sin);
    }
}