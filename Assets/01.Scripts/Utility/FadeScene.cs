using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeScene : Singleton<FadeScene>
{
    public Image fadeImage;
    public float fadeDuration = 2f;


    private void Awake()
    {
        BringFadeCanvasToFront();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BringFadeCanvasToFront();
    }

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

    private void BringFadeCanvasToFront()
    {
        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            Transform parent = canvas.transform.parent;
            if (parent != null)
            {
                Transform sangsangman = parent.Find("GameManager");
                if (sangsangman != null)
                {
                    int targetIndex = sangsangman.GetSiblingIndex();
                    canvas.transform.SetSiblingIndex(targetIndex + 1);
                }
                else
                {
                    Debug.LogWarning("GameManager를 찾지 못했습니다.");
                    canvas.transform.SetAsLastSibling(); // 못 찾으면 그냥 맨 위로
                }
            }
        }
    }
}
