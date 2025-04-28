using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float blinkSpeed = 1.5f;

    private void Update()
    {
        if (textMeshPro == null) return;

        float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);
        textMeshPro.alpha = alpha;
    }
}
