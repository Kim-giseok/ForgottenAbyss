using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public Transform target;
    public float zOffset = -10f;
    public float orthographicSize = 4f;

    public Vector2 offset = new Vector2(0f, 2f); // Y축으로 위로 올리기

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null || cam == null) return;

        // 플레이어 위치 + 오프셋 적용
        Vector3 newPos = target.position + (Vector3)offset;
        newPos.z = zOffset;
        transform.position = newPos;

        // 카메라 시야 조정
        cam.orthographicSize = orthographicSize;
    }
}
