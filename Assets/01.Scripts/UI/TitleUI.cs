using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : BaseUI
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
