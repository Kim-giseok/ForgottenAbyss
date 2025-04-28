using UnityEngine;

public class TitleUIManager : MonoBehaviour
{
    public GameObject optionWindow;
    public GameObject exitWindow;

    public void OpenOptionWindow()
    {
        optionWindow.SetActive(true);
    }

    public void OpenExitWindow()
    {
        exitWindow.SetActive(true);
    }

    public void CloseOptionWindow()
    {
        optionWindow.SetActive(false);
    }

    public void CloseExitWindow()
    {
        exitWindow.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
