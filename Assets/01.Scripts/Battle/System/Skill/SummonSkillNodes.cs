using System.Collections.Generic;
using UnityEngine;

// 액션으로 빼기
public class CancelAttached : Node
{
    public override void Start()
    {
        if (controller is not SummonController sController) { SetStatus(Status.Fail); return; }
        sController.CancelAttached();
        SetStatus(Status.Success);
    }
}

// 공격가 대쉬가 섞인 형태
// 캐릭터 비활성화로 인한 리지드 바디 직접 참조 문제 발생
// 사실상 대시 공격
public class DashAttack : Node
{
    public override void Start()
    {
        if (controller is not SummonController sController) { SetStatus(Status.Fail); return; }
        
        SoundManager.Instance.Playsfx("AgisSpell");

        controller.Anim.SetSpeed(2f);
        controller.Anim.Play("Attack");
        
        sController.Flip(Mathf.Approximately(sController.caster.eulerAngles.y, 0));
        
        // 이동 방향을 체크하도록 처리
        controller.Rigid.velocity = SummonController.InputDirection * 120f;
        controller.Rigid.drag = 20f;
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (status == AnimationStatus.Start)
        {
            BoltsPool.Instance.CreateMelee(controller.transform).SetLocalPos(Vector2.zero)
                .SetDamage(GameManager.Instance.player.playerstatus.GetStat(StatType.ATK) * 5f).SetSize(2f).Fire();
        }
        
        if (status == AnimationStatus.End)
        {
            controller.Rigid.drag = 0;
            SetStatus(Status.Success);
        }
    }

    public override void End()
    {
        controller.Anim.SetSpeed(1f);
        BoltsPool.Instance.DisableMelee(controller.transform);
    }
}

// 노드 합치기
public class Explosion : Node
{
    public override void Start()
    {
        controller.Anim.Play("Explosion");
        controller.Anim.SetSpeed(1f);
    }

    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire)
        {
            BoltsPool.Instance.CreateMelee(controller.transform).SetLocalPos(Vector2.zero).SetDamage(20).SetSize(2f).Fire();
        }
        else BoltsPool.Instance.DisableMelee(controller.transform);
    }

    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if(status == AnimationStatus.End) { SetStatus(Status.Success); return; }
    }

    public override void End()
    {
        controller.Anim.SetSpeed(1f);
    }
}

// 공격해야 다음 노드로 넘어가는 형태로 분리하기
public class ComboDashAttack : Node<SummonController>
{
    private List<string> combo = new() { "Combo1", "Combo2", "Combo3" };

    private void DashAttack(int currComboCount)
    {
        SoundManager.Instance.Playsfx("AgisSpell");
        
        controller.Anim.Play(combo[currComboCount]);

        controller.Rigid.velocity = Vector2.zero;
        controller.Rigid.gravityScale = 0f;
        controller.Rigid.drag = 10f;
        
        controller.Rigid.AddForce(SummonController.InputDirection * 120f, ForceMode2D.Impulse);
        
        // 공격 방향으로 Z축 회전
        float angle = Mathf.Atan2(SummonController.InputDirection.y, Mathf.Abs(SummonController.InputDirection.x)) * Mathf.Rad2Deg;
        controller.transform.rotation = Quaternion.Euler(0, SummonController.InputDirection.x < 0 ? 180 : 0, angle); 
        
        context.Set("combo", currComboCount + 1);
    }

    public override void Start()
    {
        context.Set("combo", 0);
        // 처음 위치 찾기 어려움
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
        if (status == AnimationStatus.Start)
        {
            BoltsPool.Instance.CreateMelee(controller.transform)
                .SetLocalPos(Vector2.zero)
                .SetDamage(20)
                .SetSize(2f)
                .Fire();
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
        
        controller.Anim.Play("Heal");
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