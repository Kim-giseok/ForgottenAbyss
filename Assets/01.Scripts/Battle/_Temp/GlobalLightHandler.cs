using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLightHandler: MonoBehaviour
{
    public static GlobalLightHandler instance { get; private set; }
    public Light2D light { get; private set; }

    private void Awake()
    {
        if (!instance) { instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
        
        light = GetComponent<Light2D>();
    }
}