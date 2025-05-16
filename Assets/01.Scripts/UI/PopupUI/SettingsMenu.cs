using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenuUI;
    private bool isSettingsOpen = false;

    private void SetSettingsMenu(bool isOpen)
    {
        isSettingsOpen = isOpen;
        settingsMenuUI.SetActive(isOpen);
        Time.timeScale = isOpen ? 0f : 1f; // 게임을 멈추거나 다시 시작함
    }

    public void ToggleSettingsMenu()
    {
        SetSettingsMenu(!isSettingsOpen);


    }

    public void CloseSettingsMenuUI()
    {
        SetSettingsMenu(false);
    }

    public void RestartStage()
    {
        CloseSettingsMenuUI();
        SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnStartScene()
    {
        CloseSettingsMenuUI();
        SceneLoader.Instance.LoadScene("Test_Title");
    }
}
