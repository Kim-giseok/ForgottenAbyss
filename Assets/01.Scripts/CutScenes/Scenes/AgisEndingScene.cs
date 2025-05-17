using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AgisEndingScene: CutScene
{
    public GameObject absorptionParticle;
    public GameObject bossApearEffect;
    
    protected override void Init()
    {
        Actions = new[]
        {
            Do(async () =>
            {
                Camera.Init();
                
                SetCutSceneMode(true);
                Player.controller.rigid.velocity = Vector2.zero;
                Player.controller.rigid.isKinematic = true;
                
                bossApearEffect.SetActive(true);
                bossApearEffect.transform.position = Player.controller.transform.position;
                
                Light.FadeOut(1, 0.4f);
                
                Sound.Playsfx("AgisApearance");
                Camera.SetNoiseProfile(SubCameraInteract.NoiseType.Held);
                Camera.SetNoise(2f, 2f);
                Player.animator.Play($"ActorHitLoop");
                Camera.Focus(Player);
                await UniTask.Delay(5000);
                bossApearEffect.SetActive(false);

                Sound.StopSFX();
                
                Camera.SetNoiseProfile(SubCameraInteract.NoiseType.Base);
                Camera.Shake(4, 4, 0.2f);
                Player.animator.Play($"ActorHit");
                
                absorptionParticle.SetActive(true);
                absorptionParticle.transform.position = Player.transform.position;
                
                // notice Agis 공격에 대한 추상화가 되어 있지 않음.
                Sound.Playsfx("AgisSpell");
                for (int i = 0; i < 9; i++)
                {
                    float angle = i * 40 * Mathf.Deg2Rad;
                    Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 32;

                    BoltsPool.Instance
                        .CreateSummon(GameManager.Instance.player.transform, SummonSkillManager.Skill.Agis)
                        .SetTrigger(true)
                        .SetCastingDirection(direction)
                        .Fire();
                }
                
                Sound.PlayBGM("Boss1EndingScene");
                SetNarration("그 순간 플레이어는 흐릿한 한 장면을 보앗다. 떠올렸다.");
                await UniTask.Delay(1000);
                Camera.SetNoise(1f, 1f);
            }),
            Do(() => {})
        };
    }
}