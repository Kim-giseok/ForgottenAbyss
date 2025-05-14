using UnityEngine;
using UnityEngine.Serialization;

public class SceneManamger: SingletonLoadRemain<SceneManamger>
{
    public LetterBox letterBox;
    public SceneCameraController CameraController { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        CameraController = GetComponent<SceneCameraController>();
    }

    public void SetCutSceneMode(bool isCutsceneMode)
    {
        if (isCutsceneMode)
        {
            UIManager.Instance.HideIngameUI();
            if(GameManager.Instance) GameManager.Instance.PausePlayer();
            Instance.letterBox.ShowLetterBox(true);
        }
        else
        {
            UIManager.Instance.ShowIngameUI();
            if(GameManager.Instance) GameManager.Instance.PausePlayer(false);
            Instance.letterBox.ShowLetterBox(false);
        }
    }
}