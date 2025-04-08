using System;
using UnityEngine;

public class NavSurface : MonoBehaviour
{
    private BoxCollider2D collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }
    
    private void Start()
    {
        Bounds bounds = collider.bounds;
        int minX = Mathf.FloorToInt(bounds.min.x);
        int maxX = Mathf.CeilToInt(bounds.max.x);
        int minY = Mathf.FloorToInt(bounds.min.y);
        int maxY = Mathf.CeilToInt(bounds.max.y);

        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                Vector2 center = new Vector2(x + 0.5f, y + 0.5f);

                // 셀마다 충돌 체크
                bool blocked = Physics2D.OverlapBox(center, Vector2.one * 0.9f, 0f);

                Debug.Log(blocked);
            }
        }
    }
}
