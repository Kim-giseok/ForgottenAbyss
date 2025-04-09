using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI dmgText;
    private float duration = 1.0f;

    public void Setup(int damage)
    {
        dmgText.text = damage.ToString();
        StartCoroutine(AnimateText());
    }

    private System.Collections.IEnumerator AnimateText()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * 1.5f;

        float startSize = 1f;
        float endSize = 0.5f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.localScale = Vector3.Lerp(Vector3.one * startSize, Vector3.one * endSize, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        DamageTextPool.Instance.ReturnToPool(gameObject);
    }
}
