using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI dmgText;
    private float duration = 1.0f;

    public void Setup(int damage)
    {
        dmgText.text = damage.ToString();

        // 데미지 별 폰트 색상 지정
        if (damage > 40)
            dmgText.color = new Color(1f, 0.2f, 0.2f); // 빨간색
        else if (damage > 30)
            dmgText.color = new Color(1f, 0.5f, 0f); // 주황색
        else if (damage > 20)
            dmgText.color = new Color(1f, 1f, 0f); // 노란색
        else
            dmgText.color = Color.white; // 기본 흰색

        // 위치 랜덤 생성
        Vector2 randomOffset = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 0.5f));
        transform.position += (Vector3)randomOffset;

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
