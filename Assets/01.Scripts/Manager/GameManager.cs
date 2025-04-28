using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Player player;
    public Camera mainCamera;
    PlayerInput input;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitReferences();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitReferences();
    }

    private void InitReferences()
    {
        player = FindObjectOfType<Player>();
        input = player.GetComponent<PlayerInput>();

        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            if (!mainCamera.TryGetComponent<CinemachineBrain>(out var brain))
                brain = mainCamera.AddComponent<CinemachineBrain>();
            brain.m_DefaultBlend.m_Time = 0f;
        }
    }

    public void PausePlayer() => input.enabled = false;
    public void ActionPlayer() => input.enabled = true;
}
