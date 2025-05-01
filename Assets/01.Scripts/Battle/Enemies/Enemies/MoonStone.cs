using System.Collections;
using UnityEngine;

public class MoonStoneWalk : Node
{
    public override void Start()
    {
        controller.animnHandler.Play("Walk");
    }

    public override void Update()
    {
        if (currTime > 2f) { SetStatus(Status.Success); return; }

        Vector2 direction = controller.Rigidbody.velocity;
        direction.x = 0.4f;
        // controller.rigidbody.velocity = direction;
    }

    public override void End()
    {
        controller.Rigidbody.velocity = Vector2.zero;
    }
}

public class MoonWarp : Node
{
    public override void Start()
    {   
        controller.animnHandler.Play("Warp");

        int platform = Random.Range(0, 7);
        var selected = NavSurface.Instance.platforms[platform].centerCell.WorldPos;
        
        // 타일 사이즈 절반 차감
        selected.y += controller.Collider.bounds.size.y;
       
        controller.transform.position = selected;
        
        for (int currDegree = 0; currDegree <= 360; currDegree += 30)
        {
            BoltsPool.Instance.Create(controller.transform, Bolts.Type.Decrescendo)
                .SetSprite("arrow").SetSize(1f).SetDamage(4).SetKnockBack(4).SetSpeed(60)
                .SetDegree(currDegree).SetDuration(0.4f).Fire();
        }
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Warp")) return;
        
        if (status == AnimationStatus.End)
        {
            SetStatus(Status.Success);
        }
    }

    public override void End()
    {
        controller.animnHandler.Play("Idle");
    }
}

public class MoonAttack1 : Node
{
    public override void Start()
    {
        controller.Rigidbody.drag = 4f;
        controller.Rigidbody.AddForce(new Vector2(16f, 0), ForceMode2D.Impulse);
        controller.animnHandler.Play("Attack");
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Attack")) return;
        if (status == AnimationStatus.End) { SetStatus(Status.Success); return; }
    }

    public override void End()
    {
        controller.Rigidbody.velocity = Vector2.zero;
        controller.Rigidbody.drag = 0f;
    }
}

public class MoonCopy1 : Node
{
    public override void Start()
    {
        foreach (Platform platform in NavSurface.Instance.platforms)
        {
            BoltsPool.Instance.CreateSummon(controller.transform, SummonSkillManager.Skill.MudWave, false).SetPosition(platform.startCell.WorldPos + new Vector2(0, 1.5f)).Fire();
        }
    }

    public override void Update()
    {
        if(currTime > 2f) { SetStatus(Status.Success); return; }
    }
}

public class MoonCopy2 : Node
{
    public override void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            float angle = i * 40 * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 32;
            BoltsPool.Instance.CreateSummon(controller.transform, SummonSkillManager.Skill.Agis, false).SetCastingDirection(direction).Fire();
        }
    }

    public override void Update()
    {
        if(currTime > 2f) { SetStatus(Status.Success); return; }
    }
}

public class MoonCopyRain3 : Node
{
    private IEnumerator FireBoltsSequentially(int count, float delay)
    {
        for (int i = 0; i < count; i++)
        {
            BoltsPool.Instance.Create(controller.transform, Bolts.Type.Rain).SetEffect(Bolts.EffectType.Penetration).Fire();
            yield return new WaitForSeconds(delay);
        }
    }
    public override void Start()
    {
        controller.StartCoroutine(FireBoltsSequentially(30, 0.1f));
    }

    public override void Update()
    {
        if(currTime > 4f) { SetStatus(Status.Success); return; }
    }
}

public class MoonFlyingMode : Node
{
    public override void Start()
    {
        controller.animnHandler.Play("Fly");
        controller.Rigidbody.gravityScale = 0;
        controller.Collider.enabled = false;

    }
    
    public override void Update()
    {
        if(currTime > 2f) { SetStatus(Status.Success); return; }

        if (controller.transform.position.z < 10)
        {
            Vector3 newPosition = controller.transform.position;
            newPosition.z += 1;
            controller.transform.position = newPosition;
        }
    }

    public override void End()
    {
        Vector2[] directions = { new(1f, 1f), new(1f, -1f), new(-1f, 1f), new(-1f, -1f), new(0f, 2f), new(2f, 0f), new(-2f, 0f), new(0f, -2f) };

        foreach (var direction in directions)
        {
            BoltsPool.Instance
                .CreateSummon(controller.transform, SummonSkillManager.Skill.MoonFlyingSummonAttack, false)
                .SetCastingDirection(direction).Fire();
        }
    }
}

public class MoonFlyingEndNode : Node
{
    
    public override void Update()
    {
        if(currTime > 2f) { SetStatus(Status.Success); return; }

        Vector3 newPosition;
        if (controller.transform.position.z > 0)
        {
            newPosition = controller.transform.position;
            newPosition.z -= 1;
            controller.transform.position = newPosition;
            return; 
        }
        
        newPosition = controller.transform.position;
        newPosition.z = 0;
        controller.transform.position = newPosition;
        SetStatus(Status.Success);
    }

    public override void End()
    {
        controller.animnHandler.Play("Land");
        controller.Collider.enabled = true;
        controller.Rigidbody.gravityScale = 2;
    }
}

public class MoonFlyingAttack : Node
{
    float fireInterval = 0.2f;

    public override void Update()
    {
        if(currTime > 4f) { SetStatus(Status.Success); return; }
        
        Vector3 targetPos = controller.agent.target.transform.position;
        targetPos.z = controller.transform.position.z;

        float speed = 3f;
        controller.transform.position = Vector3.MoveTowards(controller.transform.position, targetPos, speed * Time.deltaTime);
        
        // currTime을 fireInterval로 나눈 나머지가 0일 때마다 발사
        if (!Mathf.Approximately(Mathf.Floor(currTime / fireInterval), Mathf.Floor((currTime - Time.deltaTime) / fireInterval)))
        {
            // 발사 방향 설정
            Vector2 dir = Random.insideUnitCircle.normalized;
            // 발사
            BoltsPool.Instance
                .Create(controller.transform, Bolts.Type.Forward)
                .SetEffect(Bolts.EffectType.Penetration)
                .SetSize(1)
                .SetSprite("skeleton")
                .SetDamage(10)
                .SetDirection(dir)
                .SetDuration(5)
                .Fire();
        }
    }
}

public class MoonFlyingSummonAttack : Node
{
    float fireInterval = 0.2f;

    public override void Start()
    {
        if (controller is not SummonController sController) return; 

        controller.animnHandler.Play("Fly");
        controller.transform.SetParent(sController.caster);
    }

    public override void Update()
    {
        if (controller is not SummonController sController) return; 
        if(currTime > 4f) { SetStatus(Status.Success); return; }
        
        controller.transform.position += (Vector3)sController.castingDirection * Time.deltaTime;
        
        if (!Mathf.Approximately(Mathf.Floor(currTime / fireInterval), Mathf.Floor((currTime - Time.deltaTime) / fireInterval)))
        {
            Vector2 dir = Random.insideUnitCircle.normalized;
            BoltsPool.Instance.Create(controller.transform, Bolts.Type.Forward).SetEffect(Bolts.EffectType.Penetration)
                .SetSprite("skeleton").SetDamage(10).SetDirection(dir).SetDuration(5).Fire();
        }
    }
}
