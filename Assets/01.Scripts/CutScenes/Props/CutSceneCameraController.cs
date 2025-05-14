using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CutSceneCameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _virCam;
    private CameraZoom _camZoom;
    private CameraShake _camShake;

    private void Awake()
    {
        _virCam = FindObjectOfType<CinemachineVirtualCamera>();
        _camZoom = Camera.main!.GetComponent<CameraZoom>();
        _camShake = Camera.main!.GetComponent<CameraShake>();
    }

    public void Focus(Transform target)
    {
        // 타겟을 Follow와 LookAt에 설정
        _virCam.Follow = target;
        _virCam.LookAt = target;

        // 카메라의 위치를 타겟 위치로 직접 설정
        _virCam.transform.position = target.position;
    }

    public void Reset()
    {
        _virCam.Follow = GameManager.Instance.player.transform;
        _virCam.LookAt = GameManager.Instance.player.transform;
        
        _virCam.transform.position = GameManager.Instance.player.transform.position;
    }
    

    public void Shake(float duration = 0.1f, float amplitude = 0.2f, float frequency = 5f)
    {
        _camShake.Shake(duration, amplitude, frequency);
    }

    public void Zoom(bool isZoomIn, float zoomSpeedOverride = -1f, float moveSpeedOverride = -1f)
    {
        if (isZoomIn) _camZoom.ZoomIn(zoomSpeedOverride, moveSpeedOverride);
        else _camZoom.ZoomOut(zoomSpeedOverride, moveSpeedOverride);
    }
}