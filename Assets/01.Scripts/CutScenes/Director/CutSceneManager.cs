using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CutSceneManager: SingletonLoadRemain<CutSceneManager>
{
    public CutSceneCameraController CamController { get; private set; }

    public LetterBox LetterBox;
    public PointingComp Pointing;
    public ToolTipComp ToolTip;
    public Light2D PointLight;
    
    protected override void Awake()
    {
        base.Awake();
        CamController = GetComponent<CutSceneCameraController>();
    }
    
    public void ShowText(Transform target, string sentnece)
    {
        UIManager.Instance.OnTalk(target, sentnece);
    }

    public void SetCutSceneMode(bool isCutsceneMode)
    {
        if (isCutsceneMode)
        {
            UIManager.Instance.HideIngameUI();
            
            if(GameManager.Instance) GameManager.Instance.PausePlayer();
            Instance.LetterBox.ShowLetterBox(true);
        }
        else
        {
            UIManager.Instance.ShowIngameUI();
            
            if(GameManager.Instance) GameManager.Instance.PausePlayer(false);
            Instance.LetterBox.ShowLetterBox(false);
        }
    }
}