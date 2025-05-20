using Cysharp.Threading.Tasks;
using UnityEngine;

public class FlashbackScene: CutScene
{
    protected override async UniTask StartScene()
    {
        //[화면 전환 시작]
        await UniTask.Delay(1000);
        Scene.FadeScreen.SetFade(true, 3f);
        Sound.Playsfx("Fire_Burnning");
        
        await UniTask.Delay(1000);
        LetterBox.SetColor(Color.red);
        await Narration("잠재력 있는 씨앗이여,");

        await Wait();
    }
}