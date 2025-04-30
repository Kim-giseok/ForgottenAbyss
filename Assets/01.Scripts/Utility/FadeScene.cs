using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeScene : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f;

    public void StartFadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float elapsed = 0f;

        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(true);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            color.a = alpha;
            fadeImage.color = color;

            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }

    private IEnumerator FadeInCoroutine()
    {
        float elapsed = 0f;

        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(true);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsed / fadeDuration));
            color.a = alpha;
            fadeImage.color = color;

            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(false);
    }
}
