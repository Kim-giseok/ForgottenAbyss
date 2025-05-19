using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

// do: 전역 조명 breath 효과 넣기
public class AgisEndingScene: CutScene
{
    public GameObject absorptionParticle;
    public GameObject bossAppearEffect;
    public GameObject electronicParticle;

    public GameObject summon;
    private readonly List<GameObject> summonedObjects = new();


    public Material shinyMat;
    private Material playerMaterial;
    
    protected override async UniTask StartScene()
    {
    
        // [초기 설정]
        // 카메라 콜라이더로 인한 중앙 맞지 않는 부분에 대한 고민 필요
        // Player.transform.position = new Vector2(-3, -1);
        Camera.Init();
        Camera.DisConnect();
        playerMaterial = Player.controller.spriteRenderer.material;
        
        SetCutSceneMode(true);
        Player.controller.rigid.velocity = Vector2.zero;
        Player.controller.rigid.isKinematic = true;
        await UniTask.Delay(500);

        // [보스 효과 시작]
        Camera.Profile(SubCameraInteract.NoiseType.Held);
        Camera.Noise(2f, 2f);
        Camera.Focus(Player.transform);

        Sound.Playsfx("AgisApearance");
        Light.FadeOut(1, 0.4f);

        Player.animator.Play($"Act_Hit_Loop");

        bossAppearEffect.SetActive(true);
        bossAppearEffect.transform.position = Player.controller.transform.position + Vector3.down * 1.5f;
        
        await UniTask.Delay(5000);
        
        // [폭팔 이펙트 시작]
        Camera.Profile(SubCameraInteract.NoiseType.Base);
        Camera.Shake(4, 4, 0.2f);
        
        Player.animator.Play($"Act_Hit");
                
        Light.globalLight.intensity = 4f;
        Light.globalLight.color = Color.red;
        
        absorptionParticle.SetActive(true);
        absorptionParticle.transform.position = Player.transform.position + Vector3.up;
        electronicParticle.SetActive(true);
        Player.controller.spriteRenderer.material = shinyMat;

        bossAppearEffect.SetActive(false);
        
        Sound.StopSFX();
        Sound.Playsfx("AgisSpell");
        for (var index = 0; index < 9; index++)
        {
            var currSummon = Instantiate(summon, Player.transform.position + Vector3.down * 1.5f, Quaternion.Euler(0f, 0f, 0f));
            currSummon.transform.SetParent(transform);
            currSummon.gameObject.SetActive(true);
            var rigid = currSummon.GetComponent<Rigidbody2D>();
            
            float angle = 360f / 9 * index;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            rigid.AddForce(direction * 60f, ForceMode2D.Impulse);
            
            summonedObjects.Add(currSummon);
        }
        await UniTask.Delay(1000);

        // [회전 효과 시작]
        foreach (var obj in summonedObjects)
        {
            var orbit = obj.AddComponent<OrbitMover>();
            orbit.center = Player.transform.position + Vector3.down * 1.5f;
            orbit.angularSpeed = 90f;
        }

        // [나레이션 시작]
        Sound.PlayBGM("Boss1EndingScene");
        await Narration("그 순간, 그는 흐릿한 장면을 떠올렸다.");
        await Wait();
        
        Camera.Noise(1f, 1f);
        await Narration("그 날, 지금처럼 그림자를 흡수하여 모든 것을 파괴시키던 날을..");
        await Wait();
        
        Narration().Forget();
        await Text("이럴수가..");
        await Text("내가 그림자인건가..");
        
        LetterBox.SetColor(Color.red);
        await Narration("아지스의 음성 - 그림자여, 이미 우리는 하나의 약속된 운명을 함께할 것이다..");
        await Wait();
     
        LetterBox.SetColor(Color.white);
        await Narration("그는 진실을 알수 없는 현실 속에서 큰 혼란을 가득 품게 된다.");
        await Wait();
     
        Narration().Forget();
        await Text("도대체 그림자는 무엇이지..");
        await Text("어떻게 해야 벗어날 수 있는 것인가..");
        
        await Narration("그 순간 머릿 속에서 어떤 이의 얼굴을 희미하게 떠올렸다.");
        await Wait();
        await Narration("붉은 달이 떠오를 때, 더 붉게 비추던 존재.");
        await Wait();

    
        Narration().Forget();
        await Text("문 스톤...");
        await Text("그 존재를 없애면,");
        await Text("끝없는 반복 속에서 벗어날 수 있을거야.");
        
        await Narration("그는 자신의 존재의 소멸을 각오한 채,");
        await Wait();
        await Narration("모든 것을 끝낼 것을 결심한다.");
        await Wait();

        Light.FadeOut(3, 0.4f);
        FadeScreen.SetFade(false, 3f);
        await UniTask.Delay(3000); 

        // [초기화]
        summonedObjects.ForEach(Destroy);
        Destroy(absorptionParticle);
        Destroy(electronicParticle);
        
        Camera.Reset();
        Sound.StopBGM();
        Narration().Forget();
        SetCutSceneMode(false);
        
        Light.globalLight.intensity = 1f;
        Light.globalLight.color = Color.white;
        Player.controller.spriteRenderer.material = playerMaterial;
        Player.controller.rigid.isKinematic = false;
        
        FadeScreen.SetFade(true, 0.6f);
        Light.FadeIn(0.6f);
    }
}