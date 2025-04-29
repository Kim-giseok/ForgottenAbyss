using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightHandler: MonoBehaviour
{
    public static LightHandler Instance { get; private set; }
    
    private Light2D light;
    
    public Color startColor;
    public Color endColor;
    public float duration;

    private void Awake()
    {
        light = GetComponent<Light2D>();
    }

    private void Start()
    {
        StartCoroutine(ColorLerpLoop());
    }
    
    private IEnumerator ColorLerpLoop()
    {
        while (true)
        {
            yield return StartCoroutine(LerpColor(startColor, endColor));
            yield return StartCoroutine(LerpColor(endColor, startColor));
        }
    }
    
    private IEnumerator LerpColor(Color from, Color to)
    {
        float currTime = 0f;
        while (currTime < 1f)
        {
            currTime += Time.deltaTime / duration;
            light.color = Color.Lerp(from, to, currTime);
            yield return null;
        }
    }
}