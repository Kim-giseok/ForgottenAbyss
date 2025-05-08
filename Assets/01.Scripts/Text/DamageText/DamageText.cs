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
            targetColor = new Color(1f, 0.2f, 0.2f); // ������
        else if (damage > 30)
            targetColor = new Color(1f, 0.5f, 0f); // ��Ȳ��
        else if (damage > 20)
            targetColor = new Color(1f, 1f, 0f); // �����

        // ũ��Ƽ���� �� 
        if (isCritical)
        {
            dmgText.fontSize = defaultSize * 1.5f;

            // �ƿ����� + �۷ο� ȿ��
            dmgText.outlineWidth = 0.2f;
            dmgText.outlineColor = GetOutlineColor(targetColor);

            var mat = dmgText.fontMaterial;
            mat.EnableKeyword("GLOW_ON");
            mat.SetColor("_GlowColor", Color.white);
            mat.SetFloat("_GlowPower", 0.5f);
            mat.SetFloat("_GlowOuter", 0.5f);

            dmgText.SetAllDirty();
        }
        else
        {
            // �Ϲ� �������� ��� ȿ�� ����
            dmgText.outlineWidth = 0f;
            var mat = dmgText.fontMaterial;
            mat.DisableKeyword("GLOW_ON");

            dmgText.SetAllDirty();
        }

        StartCoroutine(AnimateText(targetColor, dmgText.fontSize, isCritical));

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
    }
}