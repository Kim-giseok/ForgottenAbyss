using Cinemachine;
using UnityEngine;

public class SceneCameraController: MonoBehaviour
{
    private CinemachineVirtualCamera _mainCam;
    public CinemachineVirtualCamera cam;

    private void Awake()
    {
        _mainCam = FindObjectOfType<CinemachineVirtualCamera>();
    }

    public void Focus(Transform target)
    {
        cam.Follow = target;
        cam.LookAt = target;

        cam.Priority = 20;
    }
}