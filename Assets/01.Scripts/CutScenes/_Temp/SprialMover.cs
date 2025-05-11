using UnityEngine;

public class SpiralMover : MonoBehaviour
{
    public float duration;
    public float spiralLoops;
    public float startRadius;
    public float startAngle;

    private Vector3 center;

    private bool isMoving;
    private float elapsed;

    private void Start()
    {
        isMoving = true;
        center = transform.position;
    }

    private void Update()
    {
        if(!isMoving) return;
        
        elapsed += Time.deltaTime;
        float normalizedTime = Mathf.Clamp01(elapsed / duration);

        float radius = Mathf.Lerp(startRadius, 0f, normalizedTime); 
        float angle = startAngle + spiralLoops * 2f * Mathf.PI * normalizedTime;
        
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        transform.position = center + new Vector3(x, y, 0f);

        if (normalizedTime >= 1f)
        {
            isMoving = false;
        }
    }
}