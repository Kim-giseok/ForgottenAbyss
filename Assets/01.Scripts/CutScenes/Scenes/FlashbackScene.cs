using Cysharp.Threading.Tasks;
using UnityEngine;

public class FlashbackScene: CutScene
{
    protected override async UniTask StartScene()
    {
        await UniTask.Delay(1000);
        UI.HideIngameUI();
        SetCutSceneMode(true);
        
        await UniTask.Delay(1000);
        LetterBox.SetColor(Color.red);
        await Narration("잠재력 있는 씨앗이여, 무엇을 비추기 위해 불을 피웠나.");

        await Wait();
    }
}