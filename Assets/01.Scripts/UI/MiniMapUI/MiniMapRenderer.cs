using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapRenderer : MonoBehaviour
{
    public Camera miniMapCamera;
    public RenderTexture miniMapTexture;

    private void Start()
    {
        if (miniMapCamera != null && miniMapTexture != null)
        {
            miniMapCamera.targetTexture = miniMapTexture;
        }
    }
}
