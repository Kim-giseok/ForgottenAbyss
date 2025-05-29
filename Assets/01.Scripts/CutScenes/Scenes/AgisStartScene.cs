using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AgisStartScene : CutScene
{
    public GameObject soundTimeline;
    public GameObject agisAppears;
    public GameObject agis;
    
    public GameObject agisActor;
    public Light2D agisActorLight;
    private SpriteRenderer agisActorSprite;
    
    protected override void PreLoad()
    {
        agisActorSprite = agisActor.GetComponent<SpriteRenderer>();
    }

    private async UniTask AgisMainActorAppears()
    {
        float duration = 2f;
        float time = 0f;

        float startIntensity = 0f;
        float endIntensity = agisActorLight.intensity;
        agisActorLight.intensity = 0f;

        Color startColor = Color.black;
        Color endColor = new Color(1f, 0f, 1f, 1f);
        agisActorSprite.color = startColor;

        while (time < duration)
        {
            float t = time / duration;

            agisActorLight.intensity = Mathf.Lerp(startIntensity, endIntensity, t);
            agisActorSprite.color = Color.Lerp(startColor, endColor, t);

            time += Time.deltaTime;
            await UniTask.Yield(); // 프레임마다 기다림
        }

        // 마지막 값 보정
        agisActorLight.intensity = endIntensity;
        agisActorSprite.color = endColor;
    }

    protected override async UniTask StartScene()
    {
        // [초기화]
        await UniTask.Delay(1000);
        Sound.StopBGM();
        
        GameManager.Instance.PausePlayer();
        SetCutSceneMode(true);
        
        Camera.Init();
        Camera.DisConnect();
        
        // [등장]
        Light.FadeOut(3, 0.2f);
        soundTimeline.SetActive(true);
        agisAppears.SetActive(true);
        
        Camera.Focus(agisAppears);
        Camera.Profile(SubCameraInteract.NoiseType.Held);
        Camera.Noise(2, 2);
        
        await UniTask.Delay(6000);
        Camera.Noise(0);
        await UniTask.Delay(2000);
        
        // [agis main actor  등장]
        soundTimeline.gameObject.SetActive(false);
        agisAppears.SetActive(false);
        agisActor.SetActive(true);

        AgisMainActorAppears().Forget();
        await UniTask.Delay(3000);

        LetterBox.SetColor(Color.red);
        // [대사 시작]
        await Narration("태양이 숨을 때, 달은 깨어난다.\n언제나 그림자가 모든 것을 삼킨다.");
        await Narration("그림자는 인간의 바닥에 새겨지고\n그들은 진실을 보지 않는다.");
        await Narration("그림자를 다루는 자는\n끝내, 그림자가 되리라.");
        
        // [초기화]
        Camera.Reset();
        Narration().Forget();
        LetterBox.SetColor(Color.white);
        Light.Reset();

        SetCutSceneMode(false);
        await UniTask.Delay(1000);
        
        GameManager.Instance.PausePlayer(false);
        
        // [Agis 생성]
        Sound.PlayBGM("BossBattle");
        agisActor.SetActive(false);
        agis.SetActive(true);
        agis.transform.SetParent(null);
        
        UI.BossHealthUI.Active(true);
        UI.BossHealthUI.SetProfile(BossProfileType.Agis);
        UI.BossHealthUI.SetPercentage(100);
    }   
}