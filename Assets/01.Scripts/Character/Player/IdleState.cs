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
        player.rigid.velocity = new Vector2(0, player.rigid.velocity.y);
    }

    public override void OnMove(Vector2 inputVec)
    {
        if (inputVec.x != 0)
        {
            player.ChangeState(PlayerState.Run);
        }
    }

    public override void OnJump()
    {
        if (player.currentJumpCount < player.jumplimit && player.isGround)
        {
            player.ChangeState(PlayerState.Jump);
        }
    }

    public override void OnAttack()
    {
        player.ChangeState(PlayerState.Attack);
    }

    public override void OnInteraction()
    {
        player.ChangeState(PlayerState.Interaction);
    }

}
