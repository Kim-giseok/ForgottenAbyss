using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineDashSlashEffect : MonoBehaviour
{
    public DashTrailEffect dashEffect;
    public ZigzagSlashEffect slashEffect;

    public float delayBetweenEffects = 0.15f;

    public void Play(Vector3 start, Vector3 direction)
    {
        StartCoroutine(PlaySequence(start, direction));
    }

    private IEnumerator PlaySequence(Vector3 start, Vector3 direction)
    {
        // 1. 대시 효과 실행
        dashEffect.Initialize(start, direction);
        yield return new WaitForSeconds(dashEffect.drawDuration + delayBetweenEffects);
        Debug.Log("Starting Zigzag effect...");
        // 2. 지그재그 베기 효과 실행
        slashEffect.Initialize(start + direction.normalized * dashEffect.length, direction);

        Destroy(gameObject, 2f);
    }
}
