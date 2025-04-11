using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;
    Camera mainCamera;

    private void Awake()
    {
        _instance = this;
        player = FindObjectOfType<Player>();

        mainCamera = Camera.main;
        if (!mainCamera.TryGetComponent<CinemachineBrain>(out var component))
            component = mainCamera.AddComponent<CinemachineBrain>();
        component.m_DefaultBlend.m_Time = 0f;
    }
}
