using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class CutScene: MonoBehaviour
{
    protected CutSceneManager Scene => CutSceneManager.Instance; 
    protected SubCameraInteract Camera => CutSceneManager.Instance.SubCams;
    protected LetterBox LetterBox => CutSceneManager.Instance.LetterBox;
    protected SoundManager Sound => SoundManager.Instance;
    protected UIManager UI => UIManager.Instance;
    protected CutUIPool UIPool => CutSceneManager.Instance.UIPool;
    protected ToolTipComp ToolTip => CutSceneManager.Instance.ToolTip;
    protected BoltsPool Projectile => BoltsPool.Instance;
    protected LightManager Light => LightManager.Instance;
    protected Player Player => GameManager.Instance.player;
    
    
    protected Action OnFinish;
    public UnityEvent onFinishUnityEvent;

    private KeyCode currInputKey = KeyCode.None;
    protected void SetInput(KeyCode newInputKey) => currInputKey = newInputKey;

    private bool stopPressed;
    private bool IsPressed { get; set; }
    
    protected async UniTask Wait()
    {
        await UniTask.WaitUntil(() => IsPressed);
    }

    protected virtual async UniTask Init() { await UniTask.Delay(1000); }

    private void Start()
    {
        _ = Init();
    }

    private void Update()
    {
        if(stopPressed) return;
        
        var currPressed = Input.anyKeyDown; 
        IsPressed = currInputKey == KeyCode.None ? (Input.anyKeyDown || Input.GetMouseButtonDown(0)) : Input.GetKeyDown(currInputKey);

        if (currPressed && !IsPressed)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            CutSceneManager.Instance.ToolTip.Shake(0.2f, 20f, 30);
        }
    }

    // 아래 부터는 참조를 직접 받아서 하도록 변경하기
    protected async UniTask Text(string texts = null, Transform newTransform = null)
    {
        if(texts == null) { UIManager.Instance.OffTalk(); return; }
        
        var currTransform = newTransform ? newTransform : GameManager.Instance.player.transform;
        UI.talkBox.transform.position = currTransform.position + new Vector3(2f, 2.6f, 0f);

        stopPressed = true;
        await UI.talkBox.Set(texts);
        stopPressed = false;
        
        await Wait();
        UI.OffTalk();
    }


    // 그냥 참조로 가져오기
    protected void SetCutSceneMode(bool isCutsceneMode) => CutSceneManager.Instance.SetMode(isCutsceneMode);
    
    // Light Manager 에서 관리하기
    protected void SetPointingLight(Transform target = null)
    {
        var pointLight = CutSceneManager.Instance.PointLight;

        if (!target)
        {
            LightManager.Instance.globalLight.color = Color.white;
            pointLight.gameObject.SetActive(false);
            return;
        }
        
        LightManager.Instance.globalLight.color = new Color32(80, 80, 80, 255);
        pointLight.gameObject.SetActive(true);
        
        // 스프라이트 피봇으로 인한 조정 값 필요
        pointLight.transform.position = target.position + Vector3.up * 0.3f;
    }
    
    // 최대한 하나로 합치기, 내부에서 처리하도록 변경하기
    protected async UniTask Narration(string newNarration = "")
    {
        if (newNarration == null)
        {
            CutSceneManager.Instance.LetterBox.narrationText.gameObject.SetActive(false);
            return;
        }
        
        stopPressed = true;
        await CutSceneManager.Instance.LetterBox.SetNarration(newNarration);
        stopPressed = false;
    }
}