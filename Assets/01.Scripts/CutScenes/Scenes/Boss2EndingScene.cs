using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Boss2EndingScene: CutScene
{ 
    public RectTransform screenshot;

    public ActorController mudEye;
    private readonly List<ActorController> mudEyes = new();
    
    
    protected override async UniTask StartScene()
    {
        // do: 앞에 죽는 모습을 좀 더 표시한 후 진행하기
        GameManager.Instance.PausePlayer();
        Player.controller.rigid.velocity = Vector2.zero;
        Player.controller.rigid.isKinematic = true;

        await UniTask.Delay(1000);
        Camera.Init();
        
        SetCutSceneMode(true); // error: 먼저 활성화되어야 Fade 가능

        // MudEye 사망 모션
        for (var index = 0; index < 32; index++)
        {
            var newPos = Player.controller.transform.position + new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
            var newMudEye = Instantiate(mudEye, newPos, Quaternion.identity);
            newMudEye.transform.localScale = new Vector3(Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f), 1);
            
            newMudEye.Anim.Play("Hit");
            mudEyes.Add(newMudEye);
        
            Camera.Shake(1, 1, 0.2f);
            Sound.Playsfx("Piano1");
        
            await UniTask.Delay(30);
        }
        
        await UniTask.Delay(1000);
        Scene.FadeScreen.SetFade(false, 3f);
        Camera.Focus(Player);
        
        
        await UniTask.Delay(5000);
        // [이미지 표시]
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

        // [초기화]
        Scene.FadeScreen.SetFade(false, 0f);
        UIPool.Delete(screenshot);
        Narration().Forget();
        
        // [화면 전환]
        await UniTask.Delay(5000);
        gameObject.SetActive(false);
        
        // 로딩이 너무 오래 걸려 차라리 로딩 페이지를 거쳐가는 게 나을 듯
        SceneLoader.Instance.LoadScene("FlashbackScene");
        // SceneManager.LoadScene("FlashbackScene");
    }
}