using UnityEngine;
using UnityEngine.EventSystems;

public class Title : MonoBehaviour
{
    public GameObject audioWindow;
    public GameObject optionWindow;
    public GameObject exitWindow;

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
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
