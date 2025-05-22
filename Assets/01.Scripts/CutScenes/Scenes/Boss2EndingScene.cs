using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Boss2EndingScene: CutScene
{ 
    public RectTransform screenshot;

    public ActorController mudEye;
    
    private readonly List<ActorController> mudEyes = new();
    private List<Light2D> mudEyesLight = new();
    
    protected override async UniTask StartScene()
    {
        // do: 앞에 죽는 모습을 좀 더 표시한 후 진행하기
        GameManager.Instance.PausePlayer();
        Player.controller.rigid.velocity = Vector2.zero;
        Player.controller.rigid.isKinematic = true;
        Light.FadeOut(1f, 0.5f);
        await UniTask.Delay(1000);
        Sound.StopBGM();
        Camera.Init();
        Camera.Focus(Player);
        
        SetCutSceneMode(true); // error: 먼저 활성화되어야 Fade 가능

        // Camera.Profile(SubCameraInteract.NoiseType.Held);
        // Camera.Shake(4, 4, 0.2f);
        // MudEye 사망 모션
        for (var index = 0; index < 24; index++)
        {
            
            Vector2 offset = new Vector2(Random.Range(-3f, 3f) + Random.Range(-1f, 1f), Random.Range(-1f, 1f) + Random.Range(-1f, 1f)) * 3f;
            
            var newPos = Player.controller.transform.position + new Vector3(offset.x, offset.y, 0);
            var newMudEye = Instantiate(mudEye, newPos, Quaternion.identity);
            newMudEye.gameObject.SetActive(true);
            newMudEye.transform.localScale = new Vector3(Random.Range(0.8f, 4f), Random.Range(0.8f, 4f), 1);
            
            newMudEye.Anim.Play("Hit");
            mudEyes.Add(newMudEye);
        
            Sound.Playsfx("Piano1");
            
            mudEyesLight.Add(newMudEye.GetComponentInChildren<Light2D>(true));
        
            await UniTask.Delay(Random.Range(30, 60));
        }
        mudEyesLight.ForEach(mudLight => mudLight.gameObject.SetActive(true));

        // Camera.Profile(SubCameraInteract.NoiseType.Base);

        await UniTask.Delay(1000);
        Scene.FadeScreen.SetFade(false, 3f);
        Camera.Focus(Player);
        
        
        await UniTask.Delay(5000);
        // [이미지 표시]
        UIPool.Set(screenshot);
        Scene.FadeScreen.SetFade(true, 5f);
        
        LetterBox.SetColor(Color.red);
        await Narration("그는 결국, 그날의 기억을 발견했다.");
        await Narration("붉은 달보다도 붉게 비추던 그림자를..");
        await Narration("그리고, 지독한 그림자를 머금어야 했던 기억을..");
        await Narration("그리고, 지켜내지 못했던 잊혀진 나락을..");

        // [초기화]
        mudEyes.ForEach(mudEye => Destroy(mudEye.gameObject));
        Camera.Reset();
        Light.FadeIn(0f);
        Scene.FadeScreen.Reset();
        Scene.GrayScreen.Reset();
        UIPool.Delete(screenshot);
        Narration().Forget();
        SetCutSceneMode(false);
        
        Player.controller.rigid.isKinematic = false;
        Player.animator.Play("Idle");
        
        gameObject.SetActive(false);
        GameManager.Instance.PausePlayer(false);
        
        // 로딩이 너무 오래 걸려 차라리 로딩 페이지를 거쳐가는 게 나을 듯
        // SceneLoader.Instance.LoadScene("FlashbackScene");
        // SceneManager.LoadScene("FlashbackScene");
    }
}