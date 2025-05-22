using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffUIElement : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image iconImage;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private CanvasGroup canvasGroup;

    private float timeLeft;
    private System.Action onEnd;
    private StatType stat;

    public void Initialize(StatType stat, float duration, Sprite icon, System.Action onEndCallback)
    {
        this.stat = stat;
        this.timeLeft = duration;
        this.onEnd = onEndCallback;

        iconImage.sprite = icon;
        timeText.text = Mathf.CeilToInt(timeLeft).ToString();

        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timeText.text = Mathf.CeilToInt(timeLeft).ToString();
            yield return null;
        }

        yield return StartCoroutine(FadeOut());
        onEnd?.Invoke();
        Destroy(gameObject);
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
