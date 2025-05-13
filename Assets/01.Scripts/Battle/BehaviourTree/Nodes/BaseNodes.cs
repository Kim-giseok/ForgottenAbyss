using UnityEngine;

public class AttackNode : Node
{
    private readonly EnemySkillCommand command;
    
    public AttackNode(EnemySkillCommand command) => this.command = command;

    public override void Start()
    {
        controller.animHandler.Play(command.animName);
        // if(currSkill.isLookTarget) controller.LookTarget();
    }
    
    public override void OnAnimatedEvent(bool isFire)
    {
        if (isFire)
        {
            command.action.Invoke();
        }
        else
        {
            BoltsPool.Instance.DisableMelee(controller.transform);
        }
    }
    
    public override void OnAnimated(AnimationStatus status, AnimatorStateInfo animInfo)
    {
        if (!animInfo.IsName(command.animName)) return;
        if (status == AnimationStatus.Start)
        {
            if (command.attackType == EnemySkillCommand.AttackType.Dash)
            {
                command.action.Invoke();
            }
        }

        if (status == AnimationStatus.End)
        {
            SetStatus(Status.Success);
        }
    }
    
    public override void End() // notice: 공격 중 피격 당하는 경우
    {
        BoltsPool.Instance.DisableMelee(controller.transform);
    }
}

public class WaitNode : Node
{
    private readonly EnemyWaitCommand currWaitCommand;
    
    public WaitNode(EnemyWaitCommand waitCommand) => this.currWaitCommand = waitCommand;

    public override void Start()
    {
        controller.Rigidbody.velocity = new Vector2(0, controller.Rigidbody.velocity.y);
        controller.animHandler.Play(currWaitCommand.animName);
    }

    public override void Update()
    {
        // 대상을 바라보게 해야하는 경우
        controller.LookTarget();
    }
}

// 특정 위치만큼 이동하거나 하는 방식도 가능해야함
public class MoveNode : Node
{
    private readonly EnemyMoveCommand currMoveCommand;
    public MoveNode(EnemyMoveCommand currMoveCommand) => this.currMoveCommand = currMoveCommand;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        // 추적 형태인 경우
        controller.Rigidbody.velocity = new Vector2(controller.agent.GetDirection().x * controller.agent.tracingSpeed, controller.Rigidbody.velocity.y);
    }

    public override void End()
    {
        base.End();
    }
}