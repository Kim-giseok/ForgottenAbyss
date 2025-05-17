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
    private Material playerMat = Player.GetComponent<Material>();
    
    protected override void Init()
    {
        Actions = new[]
        {
            Do(async () =>
            {
                // 카메라 콜라이더로 인한 중앙 맞지 않는 부분에 대한 고민 필요
                Player.transform.position = new Vector2(-3, -1);
                
                Camera.Init();
                
                SetCutSceneMode(true);
                Player.controller.rigid.velocity = Vector2.zero;
                Player.controller.rigid.isKinematic = true;
                
                bossAppearEffect.SetActive(true);
                bossAppearEffect.transform.position = Player.controller.transform.position + Vector3.down * 1.5f;
                
                Light.FadeOut(1, 0.4f);
                
                Sound.Playsfx("AgisApearance");
                Camera.SetNoiseProfile(SubCameraInteract.NoiseType.Held);
                Camera.SetNoise(2f, 2f);
                Player.animator.Play($"ActorHitLoop");
                Camera.Focus(Player.transform);
                await UniTask.Delay(5000);
                bossAppearEffect.SetActive(false);

                Sound.StopSFX();
                
                Camera.SetNoiseProfile(SubCameraInteract.NoiseType.Base);
                Camera.Shake(4, 4, 0.2f);
                Player.animator.Play($"ActorHit");
                
                absorptionParticle.SetActive(true);
                absorptionParticle.transform.position = Player.transform.position + Vector3.up * 0.8f;
                
                Light.globalLight.intensity = 4f;
                Light.globalLight.color = Color.red;
                
                electronicParticle.SetActive(true);
                Player.controller.spriteRenderer.material = shinyMat;
                
                Sound.Playsfx("AgisSpell");
                

                for (int index = 0; index < 9; index++)
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

                foreach (var obj in summonedObjects)
                {
                    var orbit = obj.AddComponent<OrbitMover>();
                    orbit.center = Player.transform.position + Vector3.down * 1.5f;
                    orbit.angularSpeed = 90f;
                }

                Sound.PlayBGM("Boss1EndingScene");
                SetNarration("그 순간, 그는 흐릿한 장면을 떠올렸다.");
                await UniTask.Delay(1000);
                Camera.SetNoise(1f, 1f);
            }),
            Do(() => SetNarration("그 날, 지금처럼 그림자를 흡수하여 모든 것을 파괴시키던 날을..")),
            Do(async () =>
            {
                SetNarration();
                SetSentence("이럴수가..");
                await UniTask.Delay(2000);
                SetSentence("내가 그림자인건가..");
                await UniTask.Delay(1000);
            }),
            Do(() =>
            {
                
                LetterBox.SetNarrationColor(Color.red);
                SetNarration("아지스의 음성: 그림자여, 이미 우리는 하나의 약속된 운명을 함께할 것이다..");
            }),
            Do(() =>
            {
                LetterBox.SetNarrationColor(Color.white);
                SetNarration("그는 진실을 알수 없는 현실 속에서 큰 혼란을 가득 품게 된다.");
            }),
            Do(async () =>
            {
                SetNarration();
                SetSentence("도대체 그림자는 무엇이지..");
                await UniTask.Delay(2000);
                SetSentence("어떻게 해야 벗어날 수 있는 것인가..");
                await UniTask.Delay(2000);
            }),
            Do(() => SetNarration("그 순간 머릿 속에서 어떤 이의 얼굴을 희미하게 떠올렸다.")),
            Do(() => SetNarration("붉은 달이 떠오를 때, 더 붉게 비추던 존재.")),
            Do(async () =>
            {
                SetNarration();
                SetSentence("문 스톤...");
                await UniTask.Delay(2000);
                SetSentence("그 존재를 없애면,");
                await UniTask.Delay(2000);
                SetSentence("끝없는 반복 속에서 벗어날 수 있을거야.");
            }),
            Do(() => SetNarration("그는 자신의 존재의 소멸을 각오한 채,")),
            Do(() => SetNarration("모든 것을 끝낼 것을 결심한다.")),
            Do(() => Light.FadeOut(1, 0.4f)),
            Do(() => Light.FadeIn(1, 0.4f)),
            Do(() => {})
        };
    }
}