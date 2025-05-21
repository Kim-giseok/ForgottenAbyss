using System.Collections;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] private ItemTooltip itemTooltip;
    [SerializeField] private float padding = 10f;
    [SerializeField] private float fadeDuration = 0.15f;

    private CanvasGroup tooltipCanvasGroup;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        tooltipCanvasGroup = itemTooltip.GetComponent<CanvasGroup>();
        if (tooltipCanvasGroup == null)
        {
            tooltipCanvasGroup = itemTooltip.gameObject.AddComponent<CanvasGroup>();
        }

        itemTooltip.Hide();
    }

    public void Show(ITooltipData data, Vector2 screenPos)
    {
        if (data == null) return;

        itemTooltip.SetData(data);
        itemTooltip.Show(screenPos); // SetActive Ã³¸®
        FadeIn();
    }

    public void Hide()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOut());
    }

    private void FadeIn()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeTo(1f));
    }

    private IEnumerator FadeOut()
    {
        yield return FadeTo(0f);
        itemTooltip.gameObject.SetActive(false);
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = tooltipCanvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            tooltipCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        tooltipCanvasGroup.alpha = targetAlpha;
    }
}
