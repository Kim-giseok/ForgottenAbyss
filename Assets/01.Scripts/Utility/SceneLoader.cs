using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonLoadRemain<SceneLoader>
{
    [HideInInspector] public string nextSceneName;
    public string loadingSceneName;

    [SerializeField] private Texture2D normalCursor;
    [SerializeField] private Texture2D clickedCursor;
    [SerializeField] private Vector2 hotSpot = Vector2.zero;

    public FadeScene fade;

    protected override void Awake()
    {
        base.Awake();

        if (normalCursor != null)
        {
            Cursor.SetCursor(normalCursor, hotSpot, CursorMode.Auto);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 클릭 시 커서 변경
        {
            Cursor.SetCursor(clickedCursor, hotSpot, CursorMode.Auto);
        }
        else if (Input.GetMouseButtonUp(0)) // 클릭 해제 시 원래 커서로 복구
        {
            Cursor.SetCursor(normalCursor, hotSpot, CursorMode.Auto);
        }
    }

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
