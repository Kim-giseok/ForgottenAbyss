using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class NPCGuideScene: CutScene
{
    public Transform npc;
    public Transform weaponBox;
    public Transform shop;

    private RectTransform menuButton;

    protected override void PreLoad()
    {
        menuButton = UIManager.Instance.GuideUI.GetComponent<RectTransform>();
    }

    protected override async UniTask StartScene()
    {
        GameManager.Instance.PausePlayer();

        Camera.Init();
        Camera.Focus(npc);
        
        Camera.Shake(2f, 2f, 0.2f);
        await Text("잠깐!", npc);
        await Text("기억을 되찾기 위해서는 만반의 준비를 해야할 거에요.", npc);
        
        // [상점 안내]
        Camera.Focus(shop);
        Pointing.Set(shop);
        SetPointingLight(shop);
        await Text("상점에는 장비,물약 등 필요한 물품들이 있어요.", npc);
       
        // [상단 스크롤 안내]
        Camera.Reset();
        Pointing.Set();
        SetPointingLight();
        await Text("우측 상단의 스크롤을 누르면 가이드를 볼 수 있어요.", npc);

        // [스탯 포인트 안내]
        await Text("레벨업을 하면 스탯 포인트로 패시브 스킬에 투자해 강해질 수 있어요.", npc);
     
        UIManager.Instance.TogglePassiveUI();
        ToolTip.Set("K 키를 눌러 스킬 포인트를 확인할 수 있습니다.", new Vector3(0, 400, 0));
        await Wait();
     
        // [무기 상자 안내]
        ToolTip.Set();
        UIManager.Instance.TogglePassiveUI();

        Camera.Focus(weaponBox);
        SetPointingLight(weaponBox);
        Pointing.Set(weaponBox);

        ToolTip.Set("무기를 장착한 이후부터 공격이 가능합니다.", new Vector3(0, 400, 0));
        await Text("무기상자에서 무기를 가져가는 것도 잊지 않도록.", npc);

        // [초기화]
        ToolTip.Set();
        Camera.Reset();
        Pointing.Set();
        SetPointingLight();
        GameManager.Instance.PausePlayer(false);
    }

}