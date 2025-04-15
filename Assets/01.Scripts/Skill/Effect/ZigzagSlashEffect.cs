using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigzagSlashEffect : MonoBehaviour
{
    public float length = 3f;             // 전체 길이
    public float duration = 0.2f;         // 전체 이펙트 실행 시간
    public int zigzagCount = 5;           // 지그재그 횟수
    public float amplitude = 0.3f;        // 지그재그 폭
    public LineRenderer line;
    public DashTrailEffect dashEffect;

    public void Initialize(Vector3 start, Vector3 direction)
    {
        Debug.Log("zigzag!");
        StartCoroutine(PlayZigzagEffect(start, direction.normalized));
    }

    private IEnumerator PlayZigzagEffect(Vector3 start, Vector3 direction)
    {
        Vector3 currentPosition = start;

        // LineRenderer 설정
        if (line == null)
        {
            line = GetComponent<LineRenderer>();
        }

        line.material = new Material(Shader.Find("Sprites/Default"));  // 기본 스프라이트 재질 사용
        line.startColor = Color.white;  // 시작 색상
        line.endColor = Color.white;    // 끝 색상
        line.startWidth = 0.1f;         // 시작 두께
        line.endWidth = 0.1f;           // 끝 두께

        // 이펙트의 진행 방향 설정
        Vector3 movementDirection = direction.normalized;

        // 이펙트 진행 거리
        float totalDistance = dashEffect.length;  // 대쉬 길이 (기존 대쉬 효과에 맞춰)
        float distanceCovered = 0f;

        // 경로를 저장할 리스트 (필요에 따라 계속 추가됨)
        List<Vector3> pathPoints = new List<Vector3>();

        // 대쉬 끝 지점에서부터 계속 진행
        while (distanceCovered < totalDistance)
        {
            // 이동 후 다음 위치 계산
            Vector3 nextPosition = currentPosition + movementDirection * Time.deltaTime * 10f;

            // 경로 추가
            pathPoints.Add(nextPosition);

            // LineRenderer 점 갱신
            line.positionCount = pathPoints.Count;

            // 각 경로에 대해 LineRenderer 위치 업데이트
            for (int i = 0; i < pathPoints.Count; i++)
            {
                line.SetPosition(i, pathPoints[i]);
            }

            // 이동 거리 업데이트
            distanceCovered += Vector3.Distance(currentPosition, nextPosition);
            currentPosition = nextPosition;

            yield return null;
        }
    }
}
