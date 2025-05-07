using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLightHandler: MonoBehaviour
{
    public static GlobalLightHandler instance { get; private set; }
    public Light2D Light { get; private set; }

    private void Awake()
    {
        if (!instance) { instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
        
        Light = GetComponent<Light2D>();
    }
}