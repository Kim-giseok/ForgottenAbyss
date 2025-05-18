using Cysharp.Threading.Tasks;
using UnityEngine;

public class Boss2EndingScene: CutScene
{
    public RectTransform Screenshot;
    
    protected override void Init()
    {
        Actions = new[]
        {
            Do(async () =>
            {
                // do: 앞에 죽는 모습을 좀 더 표시한 후 진행하기
                
                GameManager.Instance.PausePlayer();
                SetCutSceneMode(true); // error: 먼저 활성화되어야 Fade 가능
                Scene.FadeScreen.SetFade(false, 3f);
                await UniTask.Delay(3000);
                UIPool.Add(Screenshot);
                Scene.FadeScreen.SetFade(true, 3f);
                SetNarration("그는 결국, 그날의 기억을 발견했다.");
            })
        };
    }
}