using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    public override void Update()
    {
        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);

        if (!stateInfo.IsTag("Attack") && player.inputVec.x == 0)
        {
            player.ChangeState(PlayerState.Idle);
        }
    }

    public override void FixedUpdate()
    {
        float currentSpeed = player.status.stats[StatType.SPEED];

        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsTag("Attack"))
        {
            currentSpeed *= 0.2f;
        }

        player.rigid.velocity = new Vector2(player.inputVec.x * currentSpeed, player.rigid.velocity.y);
        player.UpdateDirection();
    }

    //public override void OnMove(Vector2 inputVec)
    //{
    //    AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);

    //    if (inputVec.x == 0 && !stateInfo.IsTag("Attack"))
    //    {
    //        player.ChangeState(PlayerState.Idle);
    //    }
    //}
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
}
