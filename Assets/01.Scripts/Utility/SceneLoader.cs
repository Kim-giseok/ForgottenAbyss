using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonLoadRemain<SceneLoader>
{
    [HideInInspector] public string nextSceneName;
    public string loadingSceneName;

    public FadeScene fade;

    protected override void Init()
    {
        base.Init();

        fade = GetComponent<FadeScene>();
    }

    public void LoadScene(string sceneName)
    {
        CutSceneManager.Instance.FadeScreen.gameObject.SetActive(fade);
        nextSceneName = sceneName;
        SceneManager.LoadScene(loadingSceneName);
    }
}
