using TMPro;
using UnityEngine;
using System.Collections;

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

    public void Setup(string message, Color color)
    {
        dmgText.text = message;
        dmgText.color = color;

        dmgText.fontSize = 12f;
        dmgText.fontStyle = FontStyles.Bold; // 굵게

        Camera cam = Camera.main;
        Vector3 centerPos = cam.transform.position + cam.transform.forward * 5f + cam.transform.up * -1f;
        transform.position = centerPos;

        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);

        StartCoroutine(AnimateScreenCenterText());
    }

    private IEnumerator AnimateText()
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

    public IEnumerator AnimateScreenCenterText()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        transform.localScale = Vector3.one * 1.2f;

        float duration = 1.0f;
        float holdTime = 1.5f;
        float fadeDuration = 0.5f;
        float elapsed = 0f;

        // 1. 페이드 인
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // 2. 잠시 유지
        yield return new WaitForSeconds(holdTime);

        // 3. 페이드 아웃
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        DamageTextPool.Instance.ReturnToPool(gameObject);
    }
}
