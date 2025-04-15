using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingArrowEffect : MonoBehaviour
{
    public float length = 12f;
    public float drawDuration = 0.1f; // 전체 선이 완성되는 시간
    public float tailDelay = 0.03f;   // 꼬리가 따라오는 딜레이
    public LineRenderer line;

    private Coroutine animateRoutine;

    public void Initialize(Vector3 start, Vector3 direction)
    {
        if (animateRoutine != null)
            StopCoroutine(animateRoutine);

        animateRoutine = StartCoroutine(AnimateTrail(start, direction));
    }

    private IEnumerator AnimateTrail(Vector3 start, Vector3 direction)
    {
        float elapsed = 0f;
        Vector3 end = start + direction * length;

        line.positionCount = 2;
        line.SetPosition(0, start); // tail
        line.SetPosition(1, start); // head

        while (elapsed < drawDuration)
        {
            float headT = Mathf.Clamp01(elapsed / drawDuration);
            float tailT = Mathf.Clamp01((elapsed - tailDelay) / drawDuration);

            Vector3 headPos = Vector3.Lerp(start, end, headT);
            Vector3 tailPos = Vector3.Lerp(start, end, tailT);

            line.SetPosition(0, tailPos);
            line.SetPosition(1, headPos);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 최종 위치 확정
        line.SetPosition(0, end);
        line.SetPosition(1, end);
    }
}
