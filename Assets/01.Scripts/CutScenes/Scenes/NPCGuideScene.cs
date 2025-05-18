using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class NPCGuideScene: CutScene
{
    public Transform npc;
    public Transform weaponBox;
    public Transform shop;

    private RectTransform menuButton;

    private void Start()
    {
        menuButton = UIManager.Instance.GuideUI.GetComponent<RectTransform>();
    }

    protected override void Init()
    {
        Actions = new[]
        {
            Do(() =>
            {
                CutSceneManager.Instance.SubCams.Init();
                Camera.Focus(npc);
                
                GameManager.Instance.PausePlayer();
                SetSentence("잠깐!", npc);
                Camera.Shake(2f, 2f, 0.2f);
            }),
            Do(() =>
            {
                SetSentence("기억을 되찾기 위해서는 만반의 준비를 해야할 거에요.", npc);
            }),
            Do(() =>
            {
                SetSentence("상점에는 장비,물약 등 필요한 물품들이 있어요.", npc);
                Camera.Focus(shop);
                Pointing(shop);
                SetPointingLight(shop);
            }),
            Do(() =>
            {   
                Camera.Reset();
                ResetPointer();
                ResetPointingLight();
                SetSentence("우측 상단의 스크롤을 누르면 가이드를 볼 수 있어요.", npc);
                // Pointing(menuButton);
            }),
            Do(() => SetSentence("레벨업을 하면 스탯 포인트로 패시브 스킬에 투자해 강해질 수 있어요.", npc)),
            Do(() =>
            {
                UIManager.Instance.TogglePassiveUI();
                SetToolTip(new Vector3(0, 400, 0), "K 키를 눌러 스킬 포인트를 확인할 수 있습니다.");
            }),
            Do(() =>
            {
                ResetToolTip();
                UIManager.Instance.TogglePassiveUI();  
                // Cam.Focus(weaponBox);
                SetSentence("무기상자에서 무기를 가져가는 것도 잊지 않도록.", npc);
                
                Camera.Focus(weaponBox);
                Pointing(weaponBox);
                SetPointingLight(weaponBox);
            
            }),
            // notice: 마지막 씬에서 비활성화가 되도록 되어있음(disable을 통한 리셋으로 해두는 방식도 좋을 듯)
            Do(() =>
            {
                Camera.Reset();
                ResetPointer();
                ResetPointingLight();
                GameManager.Instance.PausePlayer(false);
            }),
        };
    }

}