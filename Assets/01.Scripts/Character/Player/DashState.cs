using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : PlayerStateMachine
{
    private float dashTimer = 0f;
    public DashState(ControllerPlayer player) : base(player) { }
    public override void Enter()
    {
        player.SetInvincibility(true);
        dashTimer = 0f;

        Vector2 dashDirection = new Vector2(player.inputVec.x, 0);
        player.rigid.velocity = new Vector2(dashDirection.x * player.dashDistance / player.dashTime, player.rigid.velocity.y);
        Debug.Log($"{player.dashDistance/player.dashTime}");
    }
    public override void Update()
    {
        dashTimer += Time.deltaTime;
        if (dashTimer >= player.dashTime)
        {
            if (player.inputVec.x != 0)
                player.ChangeState(PlayerState.Run);
            else
                player.ChangeState(PlayerState.Idle);
        }
    }
    public override void Exit()
    {
        Debug.Log("Exit");
        player.SetInvincibility(false);
    }
}
