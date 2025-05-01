using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightBreath: MonoBehaviour
{
    private Light2D _light;
    
    public float min;
    public float max;
    public float speed;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
    }

    private void Update()
    {
        if (!_light) return;

        float sin = Mathf.Sin(Time.time * speed) * 0.5f + 0.5f;
        _light.pointLightOuterRadius = Mathf.Lerp(min, max, sin);
    }
}