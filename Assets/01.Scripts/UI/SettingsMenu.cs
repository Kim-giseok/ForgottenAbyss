using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsMenuUI;

    private bool isSettingsOpen = false;

    public void ToggleSettingsMenu()
    {
        isSettingsOpen = !isSettingsOpen;
        settingsMenuUI.SetActive(isSettingsOpen);

        // 옵션창이 열릴 때 게임 일시정지
        Time.timeScale = isSettingsOpen ? 0f : 1f;
    }

    public void CloseSettingsMenuUI()
    {
        isSettingsOpen = false;
        settingsMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
