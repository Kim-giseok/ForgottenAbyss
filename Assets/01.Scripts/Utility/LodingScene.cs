using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoadingScene : MonoBehaviour
{
    public Slider progressBar;
    public float minimumLoadingTime = 2f;

    private void Start()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.HideIngameUI();

        FadeScene.Instance.StartFadeIn();
        CleanUpDuplicateComponents();
        StartCoroutine(LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        string targetScene = SceneLoader.Instance.nextSceneName;
        AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene);
        operation.allowSceneActivation = false;

        float elapsedTime = 0f;

        while (elapsedTime < minimumLoadingTime)
        {
            elapsedTime += Time.deltaTime;

            if (progressBar != null)
                progressBar.value = Mathf.Lerp(0f, 1f, elapsedTime / minimumLoadingTime);

            yield return null;
        }

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        if (progressBar != null)
            progressBar.value = 1f;

        FadeScene.Instance.StartFadeOut();

        while (FadeScene.Instance.fadeImage.color.a < 1f)
        {
            yield return null;
        }

        CleanUpDuplicateComponents();

        operation.allowSceneActivation = true;

        FadeScene.Instance.StartFadeIn();

        if (UIManager.Instance != null)
            UIManager.Instance.ShowIngameUI();
    }

    private void CleanUpDuplicateComponents()
    {
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();

        if (eventSystems.Length > 1)
        {
            Debug.LogWarning($"[LoadingScene] EventSystem이 {eventSystems.Length}개 존재합니다. 정리합니다.");

            for (int i = 1; i < eventSystems.Length; i++)
            {
                Destroy(eventSystems[i].gameObject);
            }
        }

        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();
        if (audioListeners.Length > 1)
        {
            Debug.LogWarning($"AudioListener 중복 발견: {audioListeners.Length}개. 정리합니다.");

            for (int i = 1; i < audioListeners.Length; i++)
            {
                Destroy(audioListeners[i]);
            }
        }
    }
}
