using UnityEngine;
using UnityEngine.Serialization;

public class CutSceneManager: SingletonLoadRemain<CutSceneManager>
{
    public CutSceneCameraController CamController { get; private set; }
    
    public LetterBox LetterBox { get; private set; }
    public PointingComp Pointing { get; private set; }
    public ToolTipComp ToolTip { get; private set; }

    
    protected override void Awake()
    {
        base.Awake();
        CamController = GetComponent<CutSceneCameraController>();
        
        LetterBox = GetComponentInChildren<LetterBox>(true);
        Pointing = GetComponentInChildren<PointingComp>(true);
        ToolTip = GetComponentInChildren<ToolTipComp>(true);
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