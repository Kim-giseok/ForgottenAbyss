using Cysharp.Threading.Tasks;

public class EndingScene: CutScene
{
    protected override async UniTask StartScene()
    {
        Player.animator.Play("Act_Run");
        await UniTask.Delay(1000);
        UI.HideIngameUI();
        SetCutSceneMode(true);

        Narration("이렇게하면?").Forget();
        Narration("어떻게 되나?").Forget();
        
        // await Narration("이게 왜 되는 거지 원리를 모르겟네");
    }
}