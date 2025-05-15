using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : PlayerStateMachine
{
    public FallState(ControllerPlayer player) : base(player)
    {
    }

    public override void Update()
    {
        base.Update();
        if (player.isGround)
            player.ChangeState(PlayerState.Idle);
    }
}
