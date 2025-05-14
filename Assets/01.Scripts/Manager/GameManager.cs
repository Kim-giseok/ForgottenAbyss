using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : SingletonLoadRemain<GameManager>
{
    public Player player;
    public Camera mainCamera;
    PlayerInput input;
    public PlayerStatus pStatus;
    public CameraShake cameraShake;
    public CameraZoom cameraZoom;
    public bool isInputPossible = true;

    protected override void Init()
    {
        base.Init();
        player = FindObjectOfType<Player>();
        if (player == null) return;
        input = player.GetComponent<PlayerInput>();
        pStatus = player.GetComponent<PlayerStatus>();

        pStatus.OnStatPointsChanged -= UIManager.Instance.passiveUI.UpdateStatPointsUI;
        pStatus.OnStatPointsChanged += UIManager.Instance.passiveUI.UpdateStatPointsUI;

        UIManager.Instance.passiveUI.UpdateStatPointsUI(pStatus.GetAvailableStatPoints());

        mainCamera = Camera.main;
        if (mainCamera != null)
        {
            if (!mainCamera.TryGetComponent<CinemachineBrain>(out var brain))
                brain = mainCamera.AddComponent<CinemachineBrain>();
            brain.m_DefaultBlend.m_Time = 0f;

            cameraShake = mainCamera.GetComponent<CameraShake>();
            cameraZoom = mainCamera.GetComponent<CameraZoom>();
        }
    }

    public void PausePlayer(bool pause = true) => input.enabled = !pause;
}
