using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class ItemBouncing : MonoBehaviour
{
    private float duration = 0.5f;
    private float height = 2f;
    private float spread = 1.5f;
    private float rotationSpeed = Random.Range(360, 720);

    private void Start()
    {
        StartCoroutine(ThrowItem());
    }

    private IEnumerator ThrowItem()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + new Vector3(Random.Range(-spread, spread), height, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / duration;
            float curve = Mathf.Sin(t * Mathf.PI);

            transform.position = Vector3.Lerp(startPos, targetPos, curve);
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            yield return null;
        }
    }
}