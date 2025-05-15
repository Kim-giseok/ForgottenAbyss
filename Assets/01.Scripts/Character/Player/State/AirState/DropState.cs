using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropState : FallState
{
    private float fastFallSpeed = 15f;

    public DropState(ControllerPlayer player) : base(player)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.rigid.velocity = player.transform.up * -fastFallSpeed;
    }
}
