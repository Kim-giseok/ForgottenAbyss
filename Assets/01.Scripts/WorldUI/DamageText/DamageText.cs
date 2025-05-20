using TMPro;
using UnityEngine;
using System.Collections;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI dmgText;

    private Material originalMaterial;

    private float duration = 1.0f;
    private float defaultFontSize = 3.0f;

    void Awake()
    {
        originalMaterial = new Material(dmgText.fontMaterial);
    }

    public void ResetMaterial()
    {
        dmgText.fontMaterial = originalMaterial;
    }

    public void Setup(int damage, bool isCritical)
    {
        dmgText.text = damage.ToString();

        Color defaultColor = Color.white;
        Color criticalColor = new Color(1f, 0.84f, 0f);
        Color targetColor = isCritical ? criticalColor : defaultColor;

        float criticalSize = defaultFontSize * 3f;
        float targetSize = isCritical ? criticalSize : defaultFontSize;

        StartCoroutine(AnimateText(targetColor, targetSize, isCritical));

        // ��ġ ���� ����
        Vector2 randomOffset = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 1f));
        transform.position += (Vector3)randomOffset;
    }

    public void Setup(string message, Color color)
    {
        dmgText.text = message;
        dmgText.color = color;

        dmgText.fontSize = 12f;
        dmgText.fontStyle = FontStyles.Bold;

        Camera cam = Camera.main;

        Vector3 viewportCenter = new Vector3(0.5f, 0.5f, 10f);
        Vector3 worldCenter = cam.ViewportToWorldPoint(viewportCenter);

        transform.position = worldCenter;
        transform.rotation = Quaternion.LookRotation(cam.transform.forward);

        StartCoroutine(AnimateScreenCenterText());
    }

    public void ShowEXP(int experience)
    {
        dmgText.text = "+" + experience;
        dmgText.color = Color.green;
        dmgText.outlineColor = Color.white;
        
        StartCoroutine(AnimateText(Color.black, dmgText.fontSize, false));
    }

    public void ShowMessage(string message, Color? color = default)
    {
        dmgText.text = message;
        dmgText.color = color ?? Color.red;
        dmgText.fontSize = defaultFontSize;

        StartCoroutine(AnimateMessageText());
    }

    private IEnumerator AnimateText(Color targetColor, float targetFontSize, bool isCritical)
    {
        if (isCritical)
        {
            yield return StartCoroutine(PopingScale());
        }

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * 1.5f;

        float startSize = 1f;
        float endSize = 0.5f;

        float elapsed = 0f;
        Color startColor = dmgText.color;
        float startFontSize = dmgText.fontSize;

        float speedMultiplier = isCritical ? 1.25f : 1f;
        Vector3 randomYOffset = new Vector3(0, Random.Range(0f, 0.5f), 0);

        while (elapsed < duration)
        {
            float t = elapsed / duration * speedMultiplier;

            // ��ġ�� ũ�� �ִϸ��̼�
            transform.position = Vector3.Lerp(startPos + randomYOffset, endPos + randomYOffset, t);
            transform.localScale = Vector3.Lerp(Vector3.one * startSize, Vector3.one * endSize, t);

            // ����� ��Ʈ ũ�� �ִϸ��̼�
            dmgText.color = Color.Lerp(startColor, targetColor, t);
            dmgText.fontSize = Mathf.Lerp(startFontSize, targetFontSize, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // �ִϸ��̼� ���� �� ���� �� ����
        dmgText.color = targetColor;
        dmgText.fontSize = targetFontSize;

        ReturnToPool();
    }

    private IEnumerator AnimateMessageText()
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 1f;
        float fadeDuration = 0.4f;
        float holdTime = 0.4f;
        float elapsed = 0f;

        yield return new WaitForSeconds(holdTime);

        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        ReturnToPool(); // 풀로 반환
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

        yield return DamageTextManager.Instance.screenFader.FadeIn(fadeDuration);
        // 1. ���̵� ��
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(holdTime);

        // 3. ���̵� �ƿ�
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;

        yield return DamageTextManager.Instance.screenFader.FadeOut(fadeDuration);

        ReturnToPool();
    }

    private IEnumerator PopingScale()
    {
        float duration = 0.15f;
        float maxScale = 1.8f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float scale = Mathf.Lerp(maxScale, 1f, t);
            transform.localScale = Vector3.one * scale;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.one;
    }

    private Color GetOutlineColor(Color damageColor)
    {
        // ����
        if (damageColor.r > 0.9f && damageColor.g < 0.3f)
        {
            // ���� ��ο� ����
            return new Color(0.5f, 0f, 0f);
        }
        // ��Ȳ
        else if (damageColor.r > 0.9f && damageColor.g > 0.4f)
        {
            // ���� ��ο� ��Ȳ
            return new Color(0.5f, 0.25f, 0f);
        }
        // ���
        else if (damageColor.g > 0.9f)
        {
            // ���� ��ο� ���
            return new Color(0.5f, 0.5f, 0f);
        }
        // �⺻ ������� ������
        else
        {
            return Color.black;
        }
    }

    private void ReturnToPool()
    {
        DamageTextManager.Instance.pool.ReturnToPool(gameObject);
        ResetMaterial();
    }
}