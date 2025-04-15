using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraZoom : Singleton<CameraZoom>
{
    [SerializeField] private CinemachineVirtualCamera virtualCam;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomedSize = 3f;
    [SerializeField] private float defaultSize = 5f;
    [SerializeField] private float zoomSpeed = 5f;

    [Header("Camera Move During Zoom")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float zoomMoveDistance = 1.5f;
    [SerializeField] private float zoomMoveLerpSpeed = 5f;

    private Coroutine zoomRoutine;
    private Coroutine moveRoutine;

    private Vector3 originalCamPosition;
    private bool isZoomedIn = false;

    public void ZoomIn(float zoomSpeedOverride = -1f, float moveSpeedOverride = -1f)
    {
        if (isZoomedIn) return;

        originalCamPosition = virtualCam.transform.position;

        Vector3 directionToPlayer = (playerTransform.position - virtualCam.transform.position).normalized;
        Vector3 targetCamPos = virtualCam.transform.position + directionToPlayer * zoomMoveDistance;

        float zoomSpeedToUse = zoomSpeedOverride > 0f ? zoomSpeedOverride : zoomSpeed;
        float moveSpeedToUse = moveSpeedOverride > 0f ? moveSpeedOverride : zoomMoveLerpSpeed;

        StartZoom(zoomedSize, zoomSpeedToUse);
        StartMove(targetCamPos, moveSpeedToUse);

        isZoomedIn = true;
    }

    public void ZoomOut(float zoomSpeedOverride = -1f, float moveSpeedOverride = -1f)
    {
        if (!isZoomedIn) return;

        float zoomSpeedToUse = zoomSpeedOverride > 0f ? zoomSpeedOverride : zoomSpeed;
        float moveSpeedToUse = moveSpeedOverride > 0f ? moveSpeedOverride : zoomMoveLerpSpeed;

        StartZoom(defaultSize, zoomSpeedToUse);
        StartMove(originalCamPosition, moveSpeedToUse);

        isZoomedIn = false;
    }

    private void StartZoom(float targetSize, float speed)
    {
        if (zoomRoutine != null)
            StopCoroutine(zoomRoutine);

        zoomRoutine = StartCoroutine(ZoomRoutine(targetSize, speed));
    }

    private void StartMove(Vector3 targetPos, float speed)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveCameraTo(targetPos, speed));
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

    private IEnumerator MoveCameraTo(Vector3 targetPosition, float speed)
    {
        while ((virtualCam.transform.position - targetPosition).sqrMagnitude > 0.01f)
        {
            virtualCam.transform.position = Vector3.Lerp(
                virtualCam.transform.position,
                targetPosition,
                Time.deltaTime * speed
            );
            yield return null;
        }

        virtualCam.transform.position = targetPosition;
    }
}
