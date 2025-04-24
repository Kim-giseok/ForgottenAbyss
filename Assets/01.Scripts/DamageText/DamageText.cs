using TMPro;
using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI dmgText;
    private float duration = 1.0f;

    public void Setup(int damage, bool isCritical)
    {
        dmgText.text = damage.ToString();
        var defaultSize = dmgText.fontSize;

        Color targetColor = Color.white;

        if (damage > 40)
            targetColor = new Color(1f, 0.2f, 0.2f); // 빨간색
        else if (damage > 30)
            targetColor = new Color(1f, 0.5f, 0f); // 주황색
        else if (damage > 20)
            targetColor = new Color(1f, 1f, 0f); // 노란색

        // 크리티컬일 때 
        if (isCritical)
            dmgText.fontSize = defaultSize * 1.5f; // 크리티컬일 때 텍스트 크기 증가
        else
            dmgText.fontSize = defaultSize;

        StartCoroutine(AnimateText(targetColor, dmgText.fontSize));

        // 위치 랜덤 생성
        Vector2 randomOffset = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 0.5f));
        transform.position += (Vector3)randomOffset;
    }

    public void Setup(string message, Color color)
    {
        dmgText.text = message;
        dmgText.color = color;

        dmgText.fontSize = 12f;
        dmgText.fontStyle = FontStyles.Bold;

        Camera cam = Camera.main;
        Vector3 centerPos = cam.transform.position + cam.transform.forward * 5f + cam.transform.up * -1f;
        transform.position = centerPos;

        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);

        StartCoroutine(AnimateScreenCenterText());
    }

    private IEnumerator AnimateText(Color targetColor, float targetFontSize)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * 1.5f;

        float startSize = 1f;
        float endSize = 0.5f;

        float elapsed = 0f;
        Color startColor = dmgText.color;
        float startFontSize = dmgText.fontSize;

        bool isCritical = targetFontSize > startFontSize;
        float speedMultiplier = isCritical ? 2f : 1f;

        Color criticalColor = targetColor * new Color(2f, 2f, 2f);

        while (elapsed < duration)
        {
            float t = elapsed / duration * speedMultiplier;

            // 위치와 크기 애니메이션
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.localScale = Vector3.Lerp(Vector3.one * startSize, Vector3.one * endSize, t);

            // 색상과 폰트 크기 애니메이션
            dmgText.color = Color.Lerp(startColor, targetColor, t);
            dmgText.fontSize = Mathf.Lerp(startFontSize, targetFontSize, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 애니메이션 종료 후 최종 값 적용
        dmgText.color = targetColor;
        dmgText.fontSize = targetFontSize;

        ReturnToPool();
    }

    public IEnumerator AnimateScreenCenterText()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        transform.localScale = Vector3.one * 1.2f;

        float holdTime = 1.5f;
        float fadeDuration = 0.5f;
        float elapsed = 0f;

        yield return ScreenFader.Instance.FadeIn(fadeDuration);
        // 1. 페이드 인
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;

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

        yield return ScreenFader.Instance.FadeOut(fadeDuration);

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        DamageTextPool.Instance.ReturnToPool(gameObject);
    }
}