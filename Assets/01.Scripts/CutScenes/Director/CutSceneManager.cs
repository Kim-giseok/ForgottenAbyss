using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class CutSceneManager: MonoBehaviour
{
    public static CutSceneManager Instance { get; private set; }
    
    public SubCameraInteract SubCams;

    public LetterBox LetterBox;
    public TextMeshProUGUI NarrationText;

    public GameObject UIPool;
    public RectTransform UIPointingComp;
    
    public PointingComp Pointing;
    public ToolTipComp ToolTip;
    public Light2D PointLight;

    private void Awake()
    {
        if(Instance) Destroy(Instance);
        Instance = this;
        DontDestroyOnLoad(this);
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