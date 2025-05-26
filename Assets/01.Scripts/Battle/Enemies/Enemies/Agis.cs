using System.Collections;
using UnityEngine;

// 두개로 분리하기 - moveNode와 AttackNode로 분리
public class AgisSpreadShot : Node<SummonController>
{
    private readonly float duration = 0.4f;
    private readonly Vector2 direction;

    public override void Start() // 한번 더 실행하는 현상 발생
    {

        controller.Rigid.drag = 28f;

        controller.Collider.isTrigger = true;
        controller.Rigid.AddForce(controller.castingDirection * 3.8f, ForceMode2D.Impulse);
     
        // 애니메이션 도중 스프라이트 컬러 변경되지 않는 현상 발생
        controller.Render.color = Color.black;
    }

    public override void Update()
    {
        if (!(currTime >= duration)) return;
        // 데미지 체크
        var damage = controller.eController ? controller.eController.Resource.Get(EnemyStatType.Attack).value : GameManager.Instance.player.playerstatus.GetStat(StatType.ATK);
        
        const int angleStep = 20;
        const int totalAngles = 360 / angleStep;

        for (var i = 0; i < totalAngles; i++)
        {
            float angleDeg = i * angleStep;
            var angleRad = angleDeg * Mathf.Deg2Rad;

            var dir = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)).normalized;
            BoltsPool.Instance.Create(controller.transform, Bolts.Type.Linear).SetDirection(dir).SetSpeed(30f).SetDamage(damage).Fire();
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
public class AgisRainNode : Node<SummonController>
{
    private readonly float duration = 2f;
    private readonly Vector2 direction;
    

    public override void Start() // 한번 더 실행하는 현상 발생
    {
        controller.Rigid.drag = 10f;

        // 따라오지 않는 현상 수정 필요 - transform으로 처리
        controller.Rigid.AddForce(controller.castingDirection, ForceMode2D.Impulse);
     
    }

    public override void Update()
    {
        if (currTime >= duration) { SetStatus(Status.Success); return; }

        if (!Mathf.Approximately(Mathf.Floor(currTime / 0.05f), Mathf.Floor((currTime - Time.deltaTime) / 0.05f)))
        {
            BoltsPool.Instance.Create(controller.transform, Bolts.Type.Rain).SetEffect(Bolts.EffectType.Penetration).SetDamage(((SummonController)controller).eController.Resource.Get(EnemyStatType.Attack).value).Fire();
        }
    }

    public override void End()
    {
        controller.Rigid.drag = 0f;
    }
}

// 액션 노드로 활용하기
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

// board 정보로 어떻게 갱신할 수 있을까?
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

// 소환 노드로 병합 관리 가능
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