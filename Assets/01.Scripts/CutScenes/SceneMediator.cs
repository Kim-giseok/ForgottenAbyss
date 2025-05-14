using UnityEngine;
using UnityEngine.Serialization;

public class SceneMediator: SingletonLoadRemain<SceneMediator>
{
    private LetterBox _letterBox;
    private Camera mainCamera;

    protected override void Awake()
    {
        base.Awake();
        _letterBox = GetComponentInChildren<LetterBox>();
        mainCamera = Camera.main;
    }

    public void SetCutSceneMode(bool isCutsceneMode)
    {
        if (isCutsceneMode)
        {
            UIManager.Instance.HideIngameUI();
            if(GameManager.Instance) GameManager.Instance.PausePlayer();
            
            Instance._letterBox.ShowLetterBox(true);
        }
        else
        {
            UIManager.Instance.ShowIngameUI();
            if(GameManager.Instance) GameManager.Instance.PausePlayer(false);
            Instance._letterBox.ShowLetterBox(false);
        }
    }
}