using System.Collections;
using UnityEngine;

// 두개로 분리하기 - moveNode와 AttackNode로 분리
public class AgisSpreadShot : Node
{
    private readonly float duration = 1f;
    private readonly Vector2 direction;

    public override void Start() // 한번 더 실행하는 현상 발생
    {
        controller.Rigid.drag = 10f;

        // 따라오지 않는 현상 수정 필요
        if (controller is SummonController sContorller)
        {
            controller.Collider.isTrigger = true;
            controller.Rigid.AddForce(sContorller.castingDirection, ForceMode2D.Impulse);
            // controller.transform.SetParent(sContorller.eController.transform);
        }
     
        // 애니메이션 도중 스프라이트 컬러 변경되지 않는 현상 발생
        controller.Render.color = Color.black;
    }

    public override void Update()
    {
        if (!(currTime >= duration)) return;
        
        const int angleStep = 30;
        const int totalAngles = 360 / angleStep;

        for (var i = 0; i < totalAngles; i++)
        {
            float angleDeg = i * angleStep;
            var angleRad = angleDeg * Mathf.Deg2Rad;

            var dir = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)).normalized;
            BoltsPool.Instance.Create(controller.transform, Bolts.Type.Linear).SetDirection(dir).SetSpeed(16f).SetDamage(((SummonController)controller).eController.resourceHandler.Get(EnemyStatType.Attack).value).Fire();
        }

        SetStatus(Status.Success);
    }

    public override void End()
    {
        controller.Collider.isTrigger = false;
        controller.Rigid.drag = 0f;
    }
}

// 마찬가지로 2개의 노드로 분리하기
public class AgisRainNode : Node
{
    private readonly float duration = 2f;
    private readonly Vector2 direction;
    

    public override void Start() // 한번 더 실행하는 현상 발생
    {
        controller.Rigid.drag = 10f;

        // 따라오지 않는 현상 수정 필요 - transform으로 처리
        if (controller is SummonController sContorller)
        {
            controller.Rigid.AddForce(sContorller.castingDirection, ForceMode2D.Impulse);
        }
     
    }

    public override void Update()
    {
        if (currTime >= duration) { SetStatus(Status.Success); return; }

        if (!Mathf.Approximately(Mathf.Floor(currTime / 0.05f), Mathf.Floor((currTime - Time.deltaTime) / 0.05f)))
        {
            BoltsPool.Instance.Create(controller.transform, Bolts.Type.Rain).SetEffect(Bolts.EffectType.Penetration).SetDamage(((SummonController)controller).eController.resourceHandler.Get(EnemyStatType.Attack).value).Fire();
        }

    }


    public override void End()
    {
        controller.Rigid.drag = 0f;
    }
}

public class SummoningNode : Node
{
    public override void Start()
    {
        // ProjectileManager.Instance.CreateSummon(controller.transform, SummonSkillManager.Skill.BossSkill3, false);
    }

    public override void Update()
    {
        if(currTime >= 1f) { SetStatus(Status.Success); return; }
    }
}

public class SetZeroPosNode : Node
{
    public override void Start()
    {
        controller.transform.position = Vector2.zero;
        SetStatus(Status.Success);
    }
}

public class BlackHoleNode : Node
{
    public override void Start()
    {
        BoltsPool.Instance.Create(controller.transform, Bolts.Type.BlackHole).SetEffect(Bolts.EffectType.Penetration)
            .SetDirection(Vector2.down * 6f).SetDuration(4).Fire();
    }
    
    public override void Update()
    {
        if(currTime >= 2f) { SetStatus(Status.Success); return; }
    }
}

// 이 스킬이 추상화되도록 하기
public class AgisMoveNode : Node
{
    public override void Start()
    {
        controller.Anim.Play("Run");

        if (controller.Board.CurrMoveDirection == Vector2.zero)
        {
            controller.Board.CurrMoveDirection = Vector2.right;
        }
    }
    
    public override void Update()
    {
        
        if (controller.Board.CurrMoveDirection == Vector2.right && controller.transform.position.x > 8.0f)
        {
            controller.Board.CurrMoveDirection = Vector2.left;
            SetStatus(Status.Success); 
            return;
        }

        if (controller.Board.CurrMoveDirection == Vector2.left && controller.transform.position.x < -8.0f)
        {
            controller.Board.CurrMoveDirection = Vector2.right;
            SetStatus(Status.Success); 
            return;
        }
        
        controller.Board.CurrAttackTick = Mathf.FloorToInt(Time.time / 3.8f);
        if (controller.Board.CurrAttackTick != controller.Board.LastAttackTick)
        {
            controller.Board.LastAttackTick = controller.Board.CurrAttackTick;
            
            int attackType = Random.Range(0, 3);

            
            var bulletCount = 9;
            var angleStep = 360f / bulletCount;
            var radius = 42f;
            
            switch (attackType)
            {
                case 0:
                    // 방사 공격
                    // SoundManager.Instance.Playsfx("AgisSpell");
                    // for (int i = 0; i < bulletCount; i++)
                    // {
                    //     float angle = i * angleStep * Mathf.Deg2Rad;
                    //     Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                    //
                    //     // ((EnemyController)controller).statusHandler.castingDirection = direction;
                    //     BoltsPool.Instance.CreateSummon(controller.transform, SummonSkillManager.Skill.Agis)
                    //         .SetCastingDirection(direction).Fire();
                    // }
                    // break;
                case 1:
                    SoundManager.Instance.Playsfx("AgisSpell3");
                    // 블랙홀 발사
                    for (int i = 0; i < bulletCount; i++)
                    {
                        float angle = i * angleStep * Mathf.Deg2Rad;
                        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

                        
                        Debug.Log(direction);
                        BoltsPool.Instance.Create(controller.transform, Bolts.Type.BlackHole).SetEffect(Bolts.EffectType.Penetration)
                            .SetDirection(direction).SetDamage(10f).SetDuration(4).Fire();
                    }

                    break;
                case 2:
                    // SoundManager.Instance.Playsfx("AgisSpell2");
                    // // 빗물 공격
                    // Vector2[] offsets = {
                    //     new(16f, 0f),
                    //     new(48f, 0f),
                    //     new(-16f, 0f),
                    //     new(-48f, 0f),
                    // };
                    //
                    // foreach (var offset in offsets)
                    // {
                    //     BoltsPool.Instance.CreateSummon(controller.transform, SummonSkillManager.Skill.AgisRain).SetCastingDirection(offset).Fire();
                    // }
                    break;
            }
        }
        
        controller.Rigid.velocity = new Vector2(controller.Board.CurrMoveDirection.x * 4f, controller.Rigid.velocity.y);
    }

    public override void End()
    {
        controller.Rigid.velocity = Vector2.zero;
    }
}

public class AgisTriangleNode : Node
{
    public override void Start()
    {
        Vector2[] offsets = {
            new(16f, 0f),
            new(48f, 0f),
            new(-16f, 0f),
            new(-48f, 0f),
        };

        foreach (var offset in offsets)
        {
            BoltsPool.Instance.CreateSummon(controller.transform, SummonSkillManager.Skill.AgisRain).SetCastingDirection(offset).Fire();
        }
        
        SetStatus(Status.Success);
    }
}