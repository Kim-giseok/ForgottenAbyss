using System.Collections.Generic;
using UnityEngine;

public class CancelAttached : Node
{
    public override void Start()
    {
        if (controller is not SummonController sController) { SetStatus(Status.Fail); return; }
        sController.CancelAttached();
        SetStatus(Status.Success);
    }
}

// 캐릭터 비활성화로 인한 리지드 바디 직접 참조 문제 발생
public class DashAttack : Node
{
    public override void Start()
    {
        if (controller is not SummonController sController) { SetStatus(Status.Fail); return; }
        
        controller.animnHandler.SetSpeed(2f);
        controller.animnHandler.Play("Attack");
        
        context.Set("direction", sController.cRigidbody.velocity.normalized);
        sController.Flip(Mathf.Approximately(sController.caster.eulerAngles.y, 0));
        
        controller.rigidbody.velocity = sController.cRigidbody.velocity.normalized * 80f;
        controller.rigidbody.drag = 20f;

    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Attack")) return;
        
        if (status == AnimationStatus.Start)
        {
            BoltsPool.Instance.CreateMelee(controller.transform, 10f, Vector2.zero, Vector2.one * 2);
            return;
        }
        
        if (status == AnimationStatus.End)
        {
            controller.rigidbody.drag = 0;
            SetStatus(Status.Success);
        }
    }

    public override void End()
    {
        controller.animnHandler.SetSpeed(1f);
        BoltsPool.Instance.DisableMelee(controller.transform);
    }
}

public class Explosion : Node
{
    public override void Start()
    {
        if(controller is not SummonController sController) { SetStatus(Status.Fail); return; }
        sController.animnHandler.Play("Explosion");
        controller.animnHandler.SetSpeed(1f);
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire) BoltsPool.Instance.CreateMelee(controller.transform, 10f, Vector2.zero, Vector2.one * 2);
        else BoltsPool.Instance.DisableMelee(controller.transform);
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Explosion")) return;
        if(status == AnimationStatus.End) { SetStatus(Status.Success); return; }
    }

    public override void End()
    {
        controller.animnHandler.SetSpeed(1f);
    }
}

public class ComboDashAttack : Node
{
    private List<string> combo = new() { "Combo1", "Combo2", "Combo3" };

    public void DashAttack(int currComboCount)
    {
        if (controller is not SummonController sController) return;

        controller.animnHandler.Play(combo[currComboCount]);

        controller.rigidbody.velocity = Vector2.zero;
        controller.rigidbody.gravityScale = 0f;
        controller.rigidbody.drag = 4f;
        controller.rigidbody.AddForce(sController.direction * 40f, ForceMode2D.Impulse);
        
        // 공격 방향으로 Z축 회전
        float angle = Mathf.Atan2(sController.direction.y, Mathf.Abs(sController.direction.x)) * Mathf.Rad2Deg;
        controller.transform.rotation = Quaternion.Euler(0, sController.direction.x < 0 ? 180 : 0, angle); 
        
        context.Set("combo", currComboCount + 1);
    }

    public override void Start()
    {
        if (controller is not SummonController sController) return;
        
        context.Set("combo", 0);
        // 처음 위치 찾기 어려움
        sController.SecDirection(sController.cRigidbody.velocity.normalized);
        DashAttack(0);
    }
    public override void Update()
    {
        // 제한시간이 지나면 종료
        if(currTime > 3f) { SetStatus(Status.Fail); }
    }

    //do: 이동을 인식하는 것도 필요
    public override void OnPressed()
    {
        
        int currComboCount = context.Get<int>("combo");
        if (currComboCount > 2) return;
        
        DashAttack(currComboCount);
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if(!animInfo.IsName("Combo1") && !animInfo.IsName("Combo2") && !animInfo.IsName("Combo3")) return;
        
        if (status == AnimationStatus.Start)
        {
            BoltsPool.Instance.CreateMelee(controller.transform, 10f, Vector2.zero, Vector2.one * 2);
        }
        
        if(status == AnimationStatus.End)
        {
            BoltsPool.Instance.DisableMelee(controller.transform);
            
            int currComboCount = context.Get<int>("combo");

            if (currComboCount == combo.Count)
            {
                SetStatus(Status.Success);
                return;
            }
        }
    }

    public override void End()
    {
        BoltsPool.Instance.DisableMelee(controller.transform);
    }
}

// 같은 스킬인데 적과 아군의 대상이 바꾸는 부분
public class SkillHealNode : Node
{
    public override void Start()
    {
        if(controller is not SummonController sController) return;
        
        controller.animnHandler.Play("Heal");
        Collider2D[] hits = Physics2D.OverlapCircleAll(controller.transform.position, 20f, LayerMask.GetMask("Enemy"));
        foreach (var hit in hits)
        {
            if (hit.gameObject == controller.gameObject) continue;
            if (!hit.TryGetComponent(out EnemyController econtoller)) continue;

            BoltsPool.Instance.Create(controller.transform, Bolts.Type.Heal)
                .SetPosition(econtoller.transform.position)
                .SetRenderer(renderer => renderer.color = Color.green)
                .SetVFX()
                .Fire();

            econtoller.GetDamage(10);
        }
        // 체력 회복 추가
        // sController.pController.status.SetStat(StatType.CurrentHP, 10);
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName("Heal")) return;
        if(status == AnimationStatus.End) { SetStatus(Status.Success); return; }
    }
}
