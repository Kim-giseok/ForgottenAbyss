using UnityEngine;

// line renderer 이용
public class LineProjectile: MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Start()
    {
        Play(new(0, 0), new(3, 3));
    }

    private void Play(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
}