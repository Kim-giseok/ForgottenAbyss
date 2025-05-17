using UnityEngine;

public class OrbitMover : MonoBehaviour
{
    public Vector3 center;
    public float radius = 4f;
    public float angularSpeed = 40f;

    private float angle;

    void Start()
    {
        Vector2 offset = transform.position - center;
        angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        radius = offset.magnitude;
    }

    void Update()
    {
        angle += angularSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        Vector2 newPos = (Vector2)center + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
        transform.position = newPos;
    }
}