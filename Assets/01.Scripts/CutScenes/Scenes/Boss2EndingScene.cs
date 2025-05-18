using Cysharp.Threading.Tasks;
using UnityEngine;

public class Boss2EndingScene: CutScene
{
    public GameObject Screenshot;
    
    protected override void Init()
    {
        Actions = new[]
        {
            Do(async () =>
            {
                GameManager.Instance.PausePlayer();
                await UniTask.Delay(1000);
                
                SetCutSceneMode(true);
                UIPool.Add(Screenshot);
                SetNarration("그는 결국, 그날의 기억을 발견했다.");
            })
        };
    }
}