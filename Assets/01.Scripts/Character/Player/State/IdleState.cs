using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerStateMachine
{
    public IdleState(ControllerPlayer player) : base(player) { }

    public override void Enter()
    {
        //player.IgnorePlatformCollision();
        player.animator.SetBool("IsRun", false);
        player.animator.SetBool("IsJump", false);

        if(player.previousState == PlayerState.Climb)
            player.rigid.velocity = Vector2.zero;
        else
            player.rigid.velocity = new Vector2(0, player.rigid.velocity.y);

        SkillController.Instance.ResetAttack();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (player.rigid.velocity.y < -player.rigid.gravityScale * Time.fixedDeltaTime)
            player.ChangeState(PlayerState.Fall);
        else if (player.inputVec.x != 0)
            player.ChangeState(PlayerState.Run);
    }

    public override void OnJump()
    {
        if (player.CanJump && player.isGround)
        {
            player.ChangeState(PlayerState.Jump);
        }
    }

    public override void OnInteraction()
    {
        player.ChangeState(PlayerState.Interaction);
    }
}
