using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FieldItemDropAnimator: MonoBehaviour
{
    private float height = 1.2f;
    private float duration = 0.4f;
    private float xOffsetRange = 1.8f;
    
    private Coroutine animCoroutine;
    private Collider2D _collider;
    
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _collider.enabled = false;
    }

    void Start()
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(Random.Range(-xOffsetRange, xOffsetRange), 0f);
        animCoroutine = StartCoroutine(ParabolaMoveRoutine(start, end));
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

        yield return new WaitForSeconds(0.5f);
        
        StopCoroutine(animCoroutine);
        _collider.enabled = true;
    }
}