using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : PlayerStateMachine
{
    public AirState(ControllerPlayer player) : base(player)
    {
    }

    public override void FixedUpdate()
    {
        base.Update();
        player.rigid.velocity = new Vector2(player.inputVec.x * player.status.stats[StatType.SPEED], player.rigid.velocity.y);
        player.UpdateDirection();
    }

    public override void OnJump()
    {
        base.OnJump();
        if (player.inputVec.y < 0)
            player.ChangeState(PlayerState.Drop);
        else if (player.CanJump)
            player.ChangeState(PlayerState.Jump);
    }

    public override void OnDash()
    {
        base.OnDash();
        if (player.CanDash) // 빠른 낙하 중이 아닐 때만 대쉬 가능
            player.ChangeState(PlayerState.Dash);
    }
}
