using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Boss2StartScene: CutScene
{
    public List<GameObject> mudHands;
    public GameObject mudEyeActor;
    public GameObject mydEye;

    public MapSwapper MapSwapper;

    private async UniTask FocusMud(GameObject currMud)
    {
        Sound.Playsfx("MudHand_Appear");
        currMud.SetActive(true);
        Camera.Focus(currMud.transform);
        await UniTask.Delay(1000);
    }
    
    protected override async UniTask StartScene()
    {
        // [초기화]
        Sound.StopBGM();
        await UniTask.Delay(1000);
        GameManager.Instance.PausePlayer();
        Camera.Init();
        Camera.DisConnect();
        SetCutSceneMode(true);
        
        // [등장]
        Sound.Playsfx("Drone_Doom");
        Light.FadeOut(1f, 0.2f);
        foreach (var mud in mudHands) { await FocusMud(mud); }
        
        await UniTask.Delay(1000);
        
        // [머드 아이 등장]
        mudEyeActor.SetActive(true);
        Camera.Focus(mudEyeActor.transform);
        Camera.Zoom(true, 0.2f, 2);
        Sound.Playsfx("MudEye_Appear");

        await UniTask.Delay(2000);
        Camera.Zoom(true, 1f, 5);


        LetterBox.SetColor(Color.red);

        await Narration("색이란 건 섞일수록 더 아름다워질 줄 알았지만,\n 이 세상은 검게 물들고 말았네..");
        await Narration("진실을 본다고 오해하지 마라.\n 익숙함에 속아 잠들고 잠기겨라.");
        
        Camera.Focus(Player);
        Sound.Playsfx("Sword1");
        Light.FadeOut(0.2f, 0.5f);

        Narration().Forget();
        Player.animator.Play($"Act_SwordAttack_Jump");
        await Text("나는 어둠의 소산이지만, 어둠에 물들지는 않았다.");
        await Text("내가 만약 그림자가 자식이라면..");
        await Text("아버지를 베기 위해 이 곳에 왔다.");
        
        Camera.Focus(mudEyeActor);
        await Narration("그림자여, 이미 우리는 하나의 약속된 운명을 함께할 것이다.");
        await Narration("지독한 시간 속에서 서서히 잠식되리라.");
        
        Narration().Forget();
        SetCutSceneMode(false);
        await UniTask.Delay(1000);
        
        mudEyeActor.gameObject.SetActive(false);
        mydEye.SetActive(true);
        
        // [한번 섞기]
        MapSwapper.SwapMapAsync().Forget();
        // MapSwapper.isStart = true;
        
        
        UI.BossHealthUI.Active(true);
        UI.BossHealthUI.SetProfile(BossProfileType.MudEye);
        UI.BossHealthUI.SetPercentage(100);

        Camera.Reset();
        GameManager.Instance.PausePlayer(false);
        Player.animator.Play("Idle");
        Sound.PlayBGM("BossBattle");

    }
}