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

            SetCameraResolution();
        }
    }

    public void PausePlayer(bool pause = true) => input.enabled = !pause;

    void SetCameraResolution(int setWidth = 1920, int setHeight = 1080)
    {
        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            mainCamera.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            mainCamera.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }
}
