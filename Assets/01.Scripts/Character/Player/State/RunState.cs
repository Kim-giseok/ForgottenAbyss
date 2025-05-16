using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RunState : PlayerStateMachine
{
    public RunState(ControllerPlayer player) : base(player) { }
    public override void Enter()
    {
        base.Enter();
        player.animator.SetBool("IsRun", true);
    }

    public override void Exit()
    {
        base.Exit();
        player.animator.SetBool("IsRun", false);
    }

    public override void Update()
    {
        base.Update();
        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);

        if (!stateInfo.IsTag("Attack") && player.inputVec.x == 0)
        {
            player.ChangeState(PlayerState.Idle);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        float currentSpeed = player.status.stats[StatType.SPEED];

        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsTag("Attack"))
        {
            currentSpeed *= 0f;
        }

        player.rigid.velocity = new Vector2(player.inputVec.x * currentSpeed, player.rigid.velocity.y);
        player.UpdateDirection();

        if (player.rigid.velocity.y < -player.rigid.gravityScale * Time.fixedDeltaTime && !stateInfo.IsTag("Attack"))
            player.ChangeState(PlayerState.Fall);
    }

    public override void OnJump()
    {
        base.OnJump();
        if (player.CanJump && player.isGround)
            player.ChangeState(PlayerState.Jump);
    }

    public override void OnDash()
    {
        base.OnDash();
        if (player.CanDash)
            player.ChangeState(PlayerState.Dash);
    }
}
