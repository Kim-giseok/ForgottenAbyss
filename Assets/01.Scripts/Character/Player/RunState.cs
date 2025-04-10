using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : PlayerStateMachine
{
    public RunState(ControllerPlayer player) : base(player) { }
    public override void Enter()
    {
        player.animator.SetBool("IsRun", true);
    }
    public override void Exit()
    {
        player.animator.SetBool("IsRun", false);
    }
    public override void FixedUpdate()
    {
        player.rigid.velocity = new Vector2(player.inputVec.x * player.speed, player.rigid.velocity.y);
        player.UpdateDirection();
    }
    public override void OnMove(Vector2 inputVec)
    {
        if (inputVec.x == 0)
        {
            player.ChangeState(PlayerState.Idle);
        }
    }
    public override void OnJump()
    {
        if (player.currentJumpCount < player.jumplimit && player.isGround)
        {
            player.ChangeState(PlayerState.Jump);
        }
    }
    public override void OnDash()
    {
        if (player.isGround && player.inputVec.x != 0)
        {
            player.ChangeState(PlayerState.Dash);
        }
    }
    public override void OnAttack()
    {
        player.ChangeState(PlayerState.Attack);
    }
}
