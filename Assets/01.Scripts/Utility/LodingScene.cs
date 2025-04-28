using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public Slider progressBar;
    public float minimumLoadingTime = 2f;

    private void Start()
    {
        FadeScene.Instance.StartFadeIn();
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

        operation.allowSceneActivation = true;

        FadeScene.Instance.StartFadeIn();
    }
}
