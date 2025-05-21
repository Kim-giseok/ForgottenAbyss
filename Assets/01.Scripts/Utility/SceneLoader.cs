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

    // fix: 페이드 스크린 초기 활성화 오류 수정
    public void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene(loadingSceneName);
    }
}
