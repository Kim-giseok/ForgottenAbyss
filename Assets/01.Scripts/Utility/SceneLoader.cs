using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonLoadRemain<SceneLoader>
{
    [HideInInspector] public string nextSceneName;

    public void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene("Test_Loading");
    }
}
