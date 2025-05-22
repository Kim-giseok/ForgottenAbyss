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
    [SerializeField] private float idleTime = 5f; // 5초 후 숨김
    private float lastMouseMoveTime;

    public FadeScene fade;

    protected override void Awake()
    {
        base.Awake();

        if (normalCursor != null)
        {
            Cursor.SetCursor(normalCursor, hotSpot, CursorMode.Auto);
        }

        lastMouseMoveTime = Time.time;
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            lastMouseMoveTime = Time.time;
            Cursor.visible = true;
        }
        if (Time.time - lastMouseMoveTime > idleTime)
        {
            Cursor.visible = false;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Cursor.SetCursor(clickedCursor, hotSpot, CursorMode.Auto);
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
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
