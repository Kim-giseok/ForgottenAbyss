using UnityEngine;

// line renderer 이용
public class LineProjectile: MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform target;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        Play(new(0, 0), new Vector2(target.position.x, target.position.y));
    }

    private void Play(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
}