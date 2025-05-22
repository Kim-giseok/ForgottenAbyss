using UnityEngine;
using UnityEngine.EventSystems;

public class Title : MonoBehaviour
{
    public GameObject audioWindow;
    public GameObject optionWindow;
    public GameObject exitWindow;
    public GameObject infoWindow;

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
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
