using UnityEngine;

public class AttackNode : Node
{
    private readonly EnemyAttackSO _attackSO;
    
    public AttackNode(EnemyAttackSO attackSO) => this._attackSO = attackSO;

    public override void Start()
    {
        controller.animnHandler.Play(_attackSO.animName);
        // if(currSkill.isLookTarget) controller.LookTarget();
    }
    
    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire)
        {
            _attackSO.action.Invoke();
        }
        else
        {
            BoltsPool.Instance.DisableMelee(controller.transform);
        }
    }
    
    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName(_attackSO.animName)) return;
        if (status == AnimationStatus.Start)
        {
            if (_attackSO.attackType == EnemyAttackSO.AttackType.Dash)
            {
                _attackSO.action.Invoke();
            }
        }

        if (status == AnimationStatus.End)
        {
            SetStatus(Status.Success);
        }
    }
    
    public override void End() // notice: 공격 중 피격 당하는 경우
    {
        if (_attackSO.isMeleeAttack)
        {
            BoltsPool.Instance.DisableMelee(controller.transform);
        }
    }
}