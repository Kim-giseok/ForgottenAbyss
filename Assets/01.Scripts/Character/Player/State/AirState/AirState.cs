using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : PlayerStateMachine
{
    public AirState(ControllerPlayer player) : base(player)
    {
    }

    public override void OnJump()
    {
        base.OnJump();
        if (player.currentJumpCount < player.jumplimit)
            player.ChangeState(PlayerState.Jump);
    }
}
