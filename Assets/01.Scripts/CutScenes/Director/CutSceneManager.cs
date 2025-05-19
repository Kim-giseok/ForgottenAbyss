using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CutSceneManager: MonoBehaviour
{
    public static CutSceneManager Instance { get; private set; }
    public static bool isCutSceneMode;
    
    public SubCameraInteract SubCams;

    public LetterBox LetterBox;
    public TextMeshProUGUI NarrationText;

    public CutUIPool UIPool;
    public RectTransform UIPointingComp;
    
    public PointingComp Pointing;
    public ToolTipComp ToolTip;
    public Light2D PointLight;

    public FadeScreen FadeScreen;
    public Image GrayScreen;

    private void Awake()
    {
        if(Instance) Destroy(Instance);
        Instance = this;
        DontDestroyOnLoad(this);
    }
    
    public void SetMode(bool isCutsceneMode)
    {
        if (isCutsceneMode)
        {
            UIManager.Instance.HideIngameUI();
            Instance.LetterBox.Set(true);
        }
        else
        {
            UIManager.Instance.ShowIngameUI();
            Instance.LetterBox.Set(false);
        }
    }
}