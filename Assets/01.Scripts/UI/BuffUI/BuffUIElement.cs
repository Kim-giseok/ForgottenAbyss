using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffUIElement : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Image cooldownMask;
    [SerializeField] private CanvasGroup canvasGroup;

    private float duration;
    private float timeLeft;
    private System.Action onEnd;
    private StatType stat;

    public void Initialize(StatType stat, float duration, Sprite icon, System.Action onEndCallback)
    {
        this.stat = stat;
        this.timeLeft = duration;
        this.duration = duration;
        this.onEnd = onEndCallback;      

        iconImage.sprite = icon;
        timeText.text = Mathf.CeilToInt(timeLeft).ToString();
        cooldownMask.fillAmount = 1f;
        canvasGroup.alpha = 1f;

        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timeText.text = Mathf.CeilToInt(timeLeft).ToString();
            cooldownMask.fillAmount = timeLeft / duration;
            yield return null;
        }

        onEnd?.Invoke();
        yield return StartCoroutine(FadeOut());
        gameObject.SetActive(false);
    }

    private IEnumerator FadeOut()
    {
        float fadeDuration = 0.5f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = 1f - (elapsed / fadeDuration);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
