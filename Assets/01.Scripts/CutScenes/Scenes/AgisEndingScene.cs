using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AgisEndingScene: CutScene
{
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

                Camera.SetNoiseProfile(SubCameraInteract.NoiseType.Held);
                Camera.SetNoise(1f, 1f);
                Player.animator.Play($"ActorHit");
                Camera.Focus(Player);
                await UniTask.Delay(3000);
                
                Camera.SetNoiseProfile(SubCameraInteract.NoiseType.Base);
                Camera.Shake(4, 4, 0.2f);
                // notice Agis 공격에 대한 추상화가 되어 있지 않음.
                for (int i = 0; i < 9; i++)
                {
                    Sound.Playsfx("AgisSpell");
                    float angle = i * 40 * Mathf.Deg2Rad;
                    Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 32;

                    BoltsPool.Instance
                        .CreateSummon(GameManager.Instance.player.transform, SummonSkillManager.Skill.Agis)
                        .SetTrigger(true)
                        .SetCastingDirection(direction)
                        .Fire();
                }
                
                SetNarration("그 순간 플레이어는 흐릿한 한 장면을 보앗다. 떠올렸다.");
                await UniTask.Delay(1000);

            }),
        };
    }
}