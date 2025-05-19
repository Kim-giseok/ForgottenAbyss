using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerStateMachine
{
    public IdleState(ControllerPlayer player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
        player.animator.SetBool("IsRun", false);
        player.animator.SetBool("IsJump", false);

        player.rigid.velocity = new Vector2(0, player.rigid.velocity.y);

        SkillController.Instance.ResetAttack();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);

        float fallThreshold = -player.rigid.gravityScale * Time.fixedDeltaTime * 50f;

        if (player.rigid.velocity.y < fallThreshold && !stateInfo.IsTag("Attack"))
            player.ChangeState(PlayerState.Fall);
        else if (player.inputVec.x != 0)
            player.ChangeState(PlayerState.Run);
    }

    public override void OnJump()
    {
        base.OnJump();
        if (player.CanJump && player.isGround)
        {
            player.ChangeState(PlayerState.Jump);
        }
    }

    public override void OnInteraction()
    {
        base.OnInteraction();
        player.ChangeState(PlayerState.Interaction);
    }
}
