using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDrawUtil : MonoBehaviour
{
    public static void DrawCircle(Vector3 center, float radius, Color color, float duration = 0.5f, int segments = 30)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(Mathf.Cos(0), Mathf.Sin(0)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            Debug.DrawLine(prevPoint, nextPoint, color, duration);
            prevPoint = nextPoint;
        }
    }

    public static void DrawBox(Vector2 center, Vector2 size, float angle, Color color, float duration = 0.5f)
    {
        Quaternion rot = Quaternion.Euler(0, 0, angle);
        Vector2 halfSize = size / 2f;

        Vector2[] corners = new Vector2[4];
        corners[0] = center + (Vector2)(rot * new Vector3(-halfSize.x, -halfSize.y));
        corners[1] = center + (Vector2)(rot * new Vector3(-halfSize.x, halfSize.y));
        corners[2] = center + (Vector2)(rot * new Vector3(halfSize.x, halfSize.y));
        corners[3] = center + (Vector2)(rot * new Vector3(halfSize.x, -halfSize.y));

        Debug.DrawLine(corners[0], corners[1], color, duration);
        Debug.DrawLine(corners[1], corners[2], color, duration);
        Debug.DrawLine(corners[2], corners[3], color, duration);
        Debug.DrawLine(corners[3], corners[0], color, duration);
    }

    public static void DrawFan(Vector2 origin, float radius, float angle, Vector2 direction, Color color, float duration = 0.5f, int segments = 15)
    {
        direction.Normalize();
        float halfAngle = angle / 2f;
        float startAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - halfAngle;
        float angleStep = angle / segments;

        Vector3 prevPoint = origin;
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = (startAngle + i * angleStep) * Mathf.Deg2Rad;
            Vector3 nextPoint = origin + new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * radius;

            if (i > 0)
                Debug.DrawLine(prevPoint, nextPoint, color, duration);

            Debug.DrawLine(origin, nextPoint, color, duration);

            prevPoint = nextPoint;
        }
    }
}
