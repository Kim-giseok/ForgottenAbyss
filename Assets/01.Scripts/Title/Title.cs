using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject audioWindow;
    public GameObject optionWindow;
    public GameObject exitWindow;
    public GameObject infoWindow;
    public CanvasGroup exitCanvasGroup;
    public Image fadeImage;
    public float fadeDuration = 2f;

    private void Start()
    {
        CleanUpDuplicateComponents();
    }

    public void OpenAudioWindow()
    {
        audioWindow.SetActive(true);
    }

    public void OpenExitWindow()
    {
        exitCanvasGroup.alpha = 1f;
        exitWindow.SetActive(true);
    }

    public void OpenOptionWindow()
    {
        optionWindow.SetActive(true);
    }

    public void CloseAudioWindow()
    {
        audioWindow.SetActive(false);
    }

    public void CloseExitWindow()
    {
        exitWindow.SetActive(false);
    }

    public void CloseOptionWindow()
    {
        optionWindow.SetActive(false);
    }
    
    public void OpenInfoWindow()
    {
        infoWindow.SetActive(true);
    }

    public void CloseInfoWindow()
    {
        infoWindow.SetActive(false);
    }

    private void CleanUpDuplicateComponents()
    {
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();

        if (eventSystems.Length > 1)
        {
            Debug.LogWarning($"[LoadingScene] EventSystem�� {eventSystems.Length}�� �����մϴ�. �����մϴ�.");

            for (int i = 1; i < eventSystems.Length; i++)
            {
                Destroy(eventSystems[i].gameObject);
            }
        }
    }

    public void ExitGame()
    {
        StartCoroutine(DarkenUIAndExit());
    }

    private IEnumerator DarkenUIAndExit()
    {
        float elapsed = 0f;
        Color color = fadeImage.color;
        color.a = 1f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            color.a = alpha;
            fadeImage.color = color;
            yield return null;
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 게임 종료
#endif
    }
}
