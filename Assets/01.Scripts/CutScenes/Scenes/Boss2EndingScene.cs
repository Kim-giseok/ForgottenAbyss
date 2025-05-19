using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class Boss2EndingScene: CutScene
{ 
    public RectTransform screenshot;
    
    protected override async UniTask StartScene()
    {
        // do: 앞에 죽는 모습을 좀 더 표시한 후 진행하기
        GameManager.Instance.PausePlayer();
        SetCutSceneMode(true); // error: 먼저 활성화되어야 Fade 가능
        Scene.FadeScreen.SetFade(false, 3f);
        
        await UniTask.Delay(5000);
        UIPool.Set(screenshot);
        Scene.FadeScreen.SetFade(true, 3f);
        
        LetterBox.SetColor(Color.red);
        await Narration("그는 결국, 그날의 기억을 발견했다.");
        await Wait();
        await Narration("붉은 달보다도 붉게 비추던 그림자를..");
        await Wait();
        await Narration("그리고, 지독한 그림자를 머금어야 했던 기억을..");
        await Wait();
        await Narration("그리고, 지켜내지 못했던 잊혀진 나락을..");
        await Wait();
    }
}