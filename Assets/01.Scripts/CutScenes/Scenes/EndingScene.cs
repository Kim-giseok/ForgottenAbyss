using Cysharp.Threading.Tasks;
using UnityEngine;

public class EndingScene: CutScene
{
    public RectTransform endingCreditTexts;
    protected override async UniTask StartScene()
    {
        GameManager.Instance.PausePlayer();
        Player.controller.rigid.isKinematic = true;
        Player.animator.Play("Act_Run");
        // Player.controller.rigid.velocity = new Vector2(1, 0);
        await UniTask.Delay(1000);
        UI.HideIngameUI();
        SetCutSceneMode(true);
        await UniTask.Delay(2000);

        await Narration("그림자는 아마 소멸하지 않을 지 모른다.");
        await Narration("그리고 빛이 보이지 날,\n자신이 마지 그림자처럼 보일지도 모른다.");
        await Narration("하지만 상쇄 속에서\n 머지않아 다시 곧 빛날 것이다.");
        await Narration("그림자가 나 자신이 아니기에.");
        await Narration("앞으로 길에서 더 이상 나락은 존재하지 않으며,");
        await Narration("극락만이 존재할 것이다.");
        await Narration("우리에게 \"잊혀진 나락\" 이니까.");

        await Wait();
        Narration().Forget();
        UIPool.Set(endingCreditTexts);

        await Wait();
        // await Narration("이게 왜 되는 거지 원리를 모르겟네");
    }
}