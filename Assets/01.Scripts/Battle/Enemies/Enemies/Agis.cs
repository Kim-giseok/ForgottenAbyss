using UnityEngine;

public class SpreadShotNode : Node
{
    private readonly float duration = 2f;
    private readonly Vector2 direction;

    public SpreadShotNode(Vector2 direction)
    {
        this.direction = direction;
    }
    public override void Start() // 한번 더 실행하는 현상 발생
    {
        controller.rigidbody.drag = 10f;
        controller.rigidbody.AddForce(direction * 20f, ForceMode2D.Impulse);

        for (int degree = 0; degree <= 360; degree += 20)
        {
            // 튕기는 발사체가 좋을 듯
            ProjectileManager.Instance.CreateProjectile(controller.transform, 10f, degree: degree, index: 1);
        }
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
        ProjectileManager.Instance.CreateSummon(controller.transform, SummonSkillManager.Skill.BossSkill, false);
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
        ProjectileManager.Instance.CreateSummon(controller.transform, SummonSkillManager.Skill.BossSkill2, false);
        // ProjectileManager.Instance.CreateSummon(controller.transform, SummonSkillManager.Skill.BossSkill3, false);
    }

    public override void Update()
    {
        if(currTime >= 1f) { SetStatus(Status.Success); return; }
    }
}

public class MoveNode : Node
{
    private readonly float velocityX;
    public MoveNode(float velocityX) => this.velocityX = velocityX;

    public override void Start()
    {
        controller.animationHandler.Play("Run");
    }
    
    public override void Update()
    {
        float currX = controller.transform.position.x;
        
        if(velocityX == 3 && currX > 8.0f) { SetStatus(Status.Success); return; }
        if(velocityX == -3 && currX < -8.0f) { SetStatus(Status.Success); return; }
        
        if (!Mathf.Approximately(Mathf.Floor(currTime / 3), Mathf.Floor((currTime - Time.deltaTime) / 3)))
        {
            ProjectileManager.Instance.CreateSummon(controller.transform, SummonSkillManager.Skill.BossSkill, false);
        }
        
        controller.rigidbody.velocity = new Vector2(velocityX, controller.rigidbody.velocity.y);
    }
}