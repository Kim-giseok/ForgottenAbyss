using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraZoom : Singleton<CameraZoom>
{
    [SerializeField] private CinemachineVirtualCamera virtualCam;
    [SerializeField] private float zoomedSize = 3f; // 줌인할 때의 OrthoSize
    [SerializeField] private float defaultSize = 5f; // 기본 OrthoSize

    private Coroutine currentRoutine;

    public void ZoomIn(float speed = 0.2f)
    {
        StartZoom(zoomedSize, speed);
    }

    public void ZoomOut(float speed = 10f)
    {
        StartZoom(defaultSize, speed);
    }

    private void StartZoom(float targetSize, float speed)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ZoomRoutine(targetSize, speed));
    }

    private IEnumerator ZoomRoutine(float targetSize, float speed)
    {
        while (!Mathf.Approximately(virtualCam.m_Lens.OrthographicSize, targetSize))
        {
            virtualCam.m_Lens.OrthographicSize = Mathf.Lerp(
                virtualCam.m_Lens.OrthographicSize,
                targetSize,
                Time.deltaTime * speed
            );
            yield return null;
        }

        virtualCam.m_Lens.OrthographicSize = targetSize;
    }
}
