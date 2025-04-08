using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;
    void Awake()
    {
        originalPosition = transform.localPosition;
    }

    public void Shake(float duration = 0.1f, float magnitude = 0.2f)
    {
        originalPosition = transform.localPosition;
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector2 randomPoint = Random.insideUnitCircle * magnitude;
            transform.localPosition = originalPosition + new Vector3(randomPoint.x, randomPoint.y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
