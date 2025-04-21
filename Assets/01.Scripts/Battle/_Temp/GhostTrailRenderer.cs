using System.Collections.Generic;
using UnityEngine;

public class GhostTrailRenderer : MonoBehaviour
{
    private List<SpriteRenderer> spriteRenderers = new();
    private List<Vector3> lastPositions = new();

    public int frameDelay = 3;
    private int frameCounter = 0;
    
    void Start()
    {
        spriteRenderers.Add(transform.parent.GetComponent<SpriteRenderer>());

        foreach (Transform child in transform)
        {
            SpriteRenderer renderer = child.GetComponent<SpriteRenderer>();
            spriteRenderers.Add(renderer);
        }

        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            lastPositions.Add(transform.parent.position);
        }
    }
    private void FixedUpdate()
    {
        frameCounter++;
        if (frameCounter < frameDelay) return;
        frameCounter = 0;
        
        for (int i = lastPositions.Count - 1; i > 0; i--)
        {
            lastPositions[i] = lastPositions[i - 1];
        }

        // 본체의 현재 위치 저장
        lastPositions[0] = transform.parent.position;

        // 잔상 위치/스프라이트 적용 (본체는 제외)
        for (int i = 1; i < spriteRenderers.Count; i++)
        {
            spriteRenderers[i].transform.position = lastPositions[i];
            spriteRenderers[i].sprite = spriteRenderers[i - 1].sprite;
        }
    }
}