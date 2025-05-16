using Cysharp.Threading.Tasks;
using UnityEngine;

public class IntroBattleScene: CutScene
{
    protected override void Init()
    {
        Actions = new[]
        {
            Do(async () =>
            {
                GameManager.Instance.PausePlayer();
                await UniTask.Delay(1000);
                SetCutSceneMode(true);
                SetSentence("여긴 어디.. 나는 누구..");
            })
        };
    }

}