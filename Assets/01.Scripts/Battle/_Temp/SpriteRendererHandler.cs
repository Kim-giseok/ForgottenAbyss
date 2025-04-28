using UnityEngine;

public class SpriteGradientHandler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    public Color color1;
    public Color color2;
    
    public float lerpSpeed;
    private float lerpTime;
    
    private bool isLooped = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        lerpTime += Time.deltaTime * lerpSpeed;

        if (isLooped)
        {
            spriteRenderer.color = Color.Lerp(color1, color2, Mathf.PingPong(lerpTime, 1));
        }
        else
        {
            spriteRenderer.color = Color.Lerp(color2, color1, Mathf.PingPong(lerpTime, 1));
        }

        // 1초마다 색상 전환 여부 변경
        if (lerpTime > 1f)
        {
            lerpTime = 0f;
            isLooped = !isLooped;
        }
    }
}