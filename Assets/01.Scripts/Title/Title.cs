using UnityEngine;

public class Title : MonoBehaviour
{
    public GameObject audioWindow;
    public GameObject optionWindow;
    public GameObject exitWindow;

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

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
