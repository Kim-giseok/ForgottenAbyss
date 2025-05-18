using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class CutScene: MonoBehaviour
{
    protected SubCameraInteract Camera => CutSceneManager.Instance.SubCams;
    protected LetterBox LetterBox => CutSceneManager.Instance.LetterBox;
    protected SoundManager Sound => SoundManager.Instance;
    protected UIManager UI => UIManager.Instance;
    protected BoltsPool BoltsPool => BoltsPool.Instance;
    protected LightManager Light => LightManager.Instance;
    protected Player Player => GameManager.Instance.player;
    
    protected Func<UniTask>[] Actions;
    
    protected Action OnFinish;
    public UnityEvent OnFinishUnityEvent;

    private KeyCode _currInputKey = KeyCode.None;
    public void SetCurrInputKey(KeyCode newInputKey) => _currInputKey = newInputKey;
    private bool isKeyPressed => _currInputKey == KeyCode.None ? Input.anyKeyDown : Input.GetKeyDown(_currInputKey);

    protected CueMachine CueMachine { get; set; }
    
    public Func<UniTask> Do(Action action)
    {
        return () =>
        {
            action.Invoke();
            return UniTask.CompletedTask;
        };
    }

    public Func<UniTask> Do(Func<UniTask> asyncFunc)
    {
        return asyncFunc.Invoke;
    }

    protected virtual void Init() { }

    private void Awake()
    {
        OnFinish += () => gameObject.SetActive(false);
        CueMachine = new CueMachine
        {
            OnFinish = OnFinish,
            OnFinishUnityEvent = OnFinishUnityEvent
        };
        Init();
    }

    private void OnEnable()
    {
        CueMachine.Define(Actions);
        CueMachine.Start();
        CueMachine.Next();
    }

    private void Update()
    {
        if (!CueMachine.IsPlaying) return;
        
        var currKeyPressed = Input.anyKeyDown;
        
        // notice: 코드 정리 필요
        if (currKeyPressed != isKeyPressed && CutSceneManager.Instance.ToolTip.isActive)
        {
            CutSceneManager.Instance.ToolTip.Shake(0.2f, 20f, 30);
            return;
        } 
        
        if (!isKeyPressed || CueMachine.isCutSceneStarted) return;
        UIManager.Instance.OffTalk();
        CueMachine.Next();
    }
    

    protected void SetSentence(string texts, Transform newTransform = null)
    {
        CutSceneManager.Instance.ShowText(!newTransform ? GameManager.Instance.player.transform : newTransform, texts);
    }

    protected void ClearSentence()
    {
        UIManager.Instance.OffTalk();
    }
    
    protected void SetCutSceneMode(bool isCutsceneMode) => CutSceneManager.Instance.SetCutSceneMode(isCutsceneMode);

    protected void Pointing(Transform target)
    {
        SoundManager.Instance.Playsfx("Pointing");
        var pointing = CutSceneManager.Instance.Pointing;
        pointing.gameObject.SetActive(true);
        pointing.transform.position = target.position;
        pointing.SetSize(transform);
    }

    protected void ResetPointer()
    {
        CutSceneManager.Instance.Pointing.On(false);
    }

    protected void SetPointingLight(Transform target)
    {
        LightManager.Instance.globalLight.color = new Color32(80, 80, 80, 255);
        
        var light = CutSceneManager.Instance.PointLight;
        light.gameObject.SetActive(true);
        
        // 스프라이트 피봇으로 인한 조정 값 필요
        light.transform.position = target.position + Vector3.up * 0.3f;
    }

    protected void ResetPointingLight()
    {
        LightManager.Instance.globalLight.color = Color.white;
        
        var light = CutSceneManager.Instance.PointLight;
        light.gameObject.SetActive(false);
    }

    protected void SetToolTip(Vector3 newPos, string text)
    {
        CutSceneManager.Instance.ToolTip.Set(newPos, text);
    }

    protected void SetNarration(string newNarration = "")
    {
        if (newNarration == null)
        {
            CutSceneManager.Instance.LetterBox.HideNarration();
        }
        CutSceneManager.Instance.LetterBox.ShowNarration(newNarration);
    }

    // 셋 툴팁 오버로드로 정리하기
    protected void ResetToolTip()
    {
        CutSceneManager.Instance.ToolTip.gameObject.SetActive(false);

    }
}