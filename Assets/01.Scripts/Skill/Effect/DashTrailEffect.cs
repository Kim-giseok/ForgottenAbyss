using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTrailEffect : MonoBehaviour
{
    public float length = 6f;
    public float drawDuration = 0.08f;   // 전체 선이 그려지는 시간
    public float tailDelay = 0.02f;      // 꼬리 딜레이
    public LineRenderer line;

    public Coroutine animateRoutine;

    public void Initialize(Vector3 start, Vector3 direction)
    {
        if (animateRoutine != null)
            StopCoroutine(animateRoutine);
        Debug.Log("dasheffect!");
        animateRoutine = StartCoroutine(AnimateTrail(start, direction));
    }

    private IEnumerator AnimateTrail(Vector3 start, Vector3 direction)
    {
        float elapsed = 0f;
        Vector3 end = start + direction.normalized * length;

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

        line.SetPosition(0, end);
        line.SetPosition(1, end);
    }
}
