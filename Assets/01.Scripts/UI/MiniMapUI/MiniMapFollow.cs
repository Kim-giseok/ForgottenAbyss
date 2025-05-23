using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public Transform target;
    public float zOffset = -10f;
    public float orthographicSize = 4f;
    public Vector2 offset = new Vector2(0f, 2f);
    public float smoothSpeed = 5f;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null || cam == null) return;

        // 목표 위치
        Vector3 targetPosition = target.position + (Vector3)offset;
        targetPosition.z = zOffset;

        // 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // 카메라 시야 설정
        cam.orthographicSize = orthographicSize;
    }
}
