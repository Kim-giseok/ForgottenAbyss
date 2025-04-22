using UnityEngine;

public class AgisSpreadShot : Node
{
    private readonly float duration = 2f;
    private readonly Vector2 direction;

    public override void Start() // 한번 더 실행하는 현상 발생
    {
        controller.rigidbody.drag = 10f;

        // 따라오지 않는 현상 수정 필요
        if (controller is SummonController { isPlayerCaster: false } sContorller)
        {
            controller.rigidbody.AddForce(sContorller.castingDirection, ForceMode2D.Impulse);
            controller.transform.SetParent(sContorller.eController.transform);
        }
        

        // for (int degree = 0; degree <= 360; degree += 20)
        // {
            // 튕기는 발사체가 좋을 듯
        // }
    }

    public override void Update()
    {
        if(currTime >= duration) { SetStatus(Status.Success); return; }
    }

    public override void End()
    {
        controller.rigidbody.drag = 0f;
    }
}

public class SummoningAllNode : Node
{
    public override void Start()
    {
        BoltsPool.Instance.CreateSummon(controller.transform, SummonSkillManager.Skill.Agis, false);
    }

    public override void Update()
    {
        if(currTime >= 1f) { SetStatus(Status.Success); return; }
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

public class TeleportNode : Node
{
    
}

public class MoveNode : Node
{
    private readonly float velocityX;
    public MoveNode(float velocityX) => this.velocityX = velocityX;

    public override void Start()
    {
        controller.animnHandler.Play("Run");
    }
    
    public override void Update()
    {
        float currX = controller.transform.position.x;
        
        if(velocityX == 3 && currX > 8.0f) { SetStatus(Status.Success); return; }
        if(velocityX == -3 && currX < -8.0f) { SetStatus(Status.Success); return; }
        
        if (!Mathf.Approximately(Mathf.Floor(currTime / 3), Mathf.Floor((currTime - Time.deltaTime) / 3)))
        {
            int bulletCount = 9;
            float angleStep = 360f / bulletCount;
            float radius = 32f;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

                ((EnemyController)controller).statusHandler.castingDirection = dir;
                BoltsPool.Instance.CreateSummon(controller.transform, SummonSkillManager.Skill.Agis);
            }
            
        }
        
        controller.rigidbody.velocity = new Vector2(velocityX, controller.rigidbody.velocity.y);
    }

    public override void End()
    {
        controller.rigidbody.velocity = Vector2.zero;
    }
}