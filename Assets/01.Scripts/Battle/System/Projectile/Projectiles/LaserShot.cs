using UnityEngine;

public class LaserShot: Projectile
{
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Update()
    {
    }

    private void Draw(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
}