using System.Collections;
using UnityEngine;

public class EnemyAgis
{
    // public Skills skills
}

// 두개로 분리하기
public class AgisSpreadShot : Node
{
    private readonly float duration = 2f;
    private readonly Vector2 direction;

    public override void Start() // 한번 더 실행하는 현상 발생
    {
        controller.Rigidbody.drag = 10f;

        // 따라오지 않는 현상 수정 필요
        if (controller is SummonController sContorller)
        {
            controller.Collider.isTrigger = true;
            controller.Rigidbody.AddForce(sContorller.castingDirection, ForceMode2D.Impulse);
            // controller.transform.SetParent(sContorller.eController.transform);
        }
     
        // 애니메이션 도중 스프라이트 컬러 변경되지 않는 현상 발생
        controller.Renderer.color = Color.black;
    }

    public override void Update()
    {
        if (currTime >= duration)
        {
            Vector2[] directions = { Vector2.up, Vector2.down, Vector2.right, Vector2.left };

            foreach (var dir in directions)
            {
                BoltsPool.Instance
                    .Create(controller.transform, Bolts.Type.Linear)
                    .SetDirection(dir)
                    .SetSpeed(16f)
                    .SetDamage(10)
                    .Fire();
            }
            SetStatus(Status.Success); return;
        }
    }

    public override void End()
    {
        controller.Collider.isTrigger = false;
        controller.Rigidbody.drag = 0f;
    }
}

public class AgisRainNode : Node
{
    private readonly float duration = 2f;
    private readonly Vector2 direction;
    

    public override void Start() // 한번 더 실행하는 현상 발생
    {
        controller.Rigidbody.drag = 10f;

        // 따라오지 않는 현상 수정 필요 - transform으로 처리
        if (controller is SummonController sContorller)
        {
            controller.Rigidbody.AddForce(sContorller.castingDirection, ForceMode2D.Impulse);
        }
     
    }

    public override void Update()
    {
        if (currTime >= duration) { SetStatus(Status.Success); return; }

        if (!Mathf.Approximately(Mathf.Floor(currTime / 0.2f), Mathf.Floor((currTime - Time.deltaTime) / 0.2f)))
        {
            BoltsPool.Instance.Create(controller.transform, Bolts.Type.Rain).SetEffect(Bolts.EffectType.Penetration).SetDamage(8).Fire();
        }

    }


    public override void End()
    {
        controller.Rigidbody.drag = 0f;
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
        controller.animHandler.Play("Run");

        if (controller.board.currMoveDirection == Vector2.zero)
        {
            controller.board.currMoveDirection = Vector2.right;
        }
    }
    
    public override void Update()
    {
        
        if (controller.board.currMoveDirection == Vector2.right && controller.transform.position.x > 8.0f)
        {
            controller.board.currMoveDirection = Vector2.left;
            SetStatus(Status.Success); 
            return;
        }

        if (controller.board.currMoveDirection == Vector2.left && controller.transform.position.x < -8.0f)
        {
            controller.board.currMoveDirection = Vector2.right;
            SetStatus(Status.Success); 
            return;
        }
        
        controller.board.currAttackTick = Mathf.FloorToInt(Time.time / 3.8f);
        if (controller.board.currAttackTick != controller.board.lastAttackTick)
        {
            controller.board.lastAttackTick = controller.board.currAttackTick;
            
            int attackType = Random.Range(0, 3);

            switch (attackType)
            {
                case 0:
                    // 방사 공격
                    int bulletCount = 9;
                    float angleStep = 360f / bulletCount;
                    float radius = 32f;

                    for (int i = 0; i < bulletCount; i++)
                    {
                        float angle = i * angleStep * Mathf.Deg2Rad;
                        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

                        // ((EnemyController)controller).statusHandler.castingDirection = direction;
                        BoltsPool.Instance.CreateSummon(controller.transform, SummonSkillManager.Skill.Agis)
                            .SetCastingDirection(direction).Fire();
                    }
                    break;
                case 1:
                    // 블랙홀 발사
                    BoltsPool.Instance.Create(controller.transform, Bolts.Type.BlackHole).SetEffect(Bolts.EffectType.Penetration)
                        .SetDirection(Vector2.down * 6f).SetDamage(10f).SetDuration(4).Fire();
                    break;
                case 2:
                    // 빗물 공격
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
                    break;
            }
        }
        
        controller.Rigidbody.velocity = new Vector2(controller.board.currMoveDirection.x * 4f, controller.Rigidbody.velocity.y);
    }

    public override void End()
    {
        controller.Rigidbody.velocity = Vector2.zero;
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