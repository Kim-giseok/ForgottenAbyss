using System;
using UnityEngine;

public class NPCGuideScene: CutScene
{
    public Transform weaponBox;
    public Transform shop;

    private RectTransform menuButton;

    private void Start()
    {
        menuButton = UIManager.Instance.GuideUI.GetComponent<RectTransform>();
    }

    protected override void Init()
    {
        Actions = new Action[]
        {
            () =>
            {
                GameManager.Instance.PausePlayer();
                SetSentence("잠깐");
            },
            () => SetSentence("기억을 되찾기 위해서는 만반의 준비를 해야할 거야"),
            () =>
            {
                SetSentence("상점에는 장비,물약 등 필요한 물품들이 있지");
                // Cam.Focus(shop);
                Pointing(shop);
                SetPointingLight(shop);
            },
            () =>
            {   
                ResetPointer();
                ResetPointingLight();
                SetSentence("우측 상단의 스크롤을 누르면 가이드를 볼 수 있을거야");
                // Pointing(menuButton);
            },
            () => SetSentence("레벨업을 하면 스탯 포인트로 패시브 스킬에 투자해 강해질 수 있네"),
            () =>
            {
                UIManager.Instance.TogglePassiveUI();
                SetToolTip(new Vector3(0, 400, 0), "K 키를 눌러 스킬 포인트를 확인할 수 있습니다.");
            },
            () =>
            {
                ResetToolTip();
                UIManager.Instance.TogglePassiveUI();  
                // Cam.Focus(weaponBox);
                SetSentence("무기상자에서 무기를 가져가는 것도 잊지말게");
                
                Pointing(weaponBox);
                SetPointingLight(weaponBox);

            },
            // notice: 마지막 씬에서 비활성화가 되도록 되어있음(disable을 통한 리셋으로 해두는 방식도 좋을 듯)
            () =>
            {
                ResetPointer();
                ResetPointingLight();
                GameManager.Instance.PausePlayer(false);
            },
        };
    }

}