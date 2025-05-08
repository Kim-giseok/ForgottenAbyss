using System.Collections;
using UnityEngine;

public class FieldItemDropAnimator: MonoBehaviour
{
    private float height = 1.2f;
    private float duration = 0.4f;
    private float xOffsetRange = 1.8f;

    void Start()
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(Random.Range(-xOffsetRange, xOffsetRange), 0f);
        StartCoroutine(ParabolaMoveRoutine(start, end));
    }

    IEnumerator ParabolaMoveRoutine(Vector2 endPoint, Vector2 startPoint)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float progress = time / duration;

            float x = Mathf.Lerp(endPoint.x, startPoint.x, progress);
            float y = endPoint.y + height * 4f * (progress - progress * progress);

            transform.position = new Vector3(x, y);
            yield return null;
        }

        transform.position = startPoint;
    }
}