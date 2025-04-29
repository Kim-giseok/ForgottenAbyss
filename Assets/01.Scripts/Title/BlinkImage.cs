using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlinkImage : MonoBehaviour
{
    public Image targetImage;
    public float blinkSpeed = 1.5f;

    private void Update()
    {
        if (targetImage == null) return;

        Color color = targetImage.color;
        color.a = Mathf.PingPong(Time.time * blinkSpeed, 1f);
        targetImage.color = color;
    }
}
