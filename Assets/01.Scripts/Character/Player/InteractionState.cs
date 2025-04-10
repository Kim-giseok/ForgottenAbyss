using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionState : PlayerStateMachine
{
    public InteractionState(ControllerPlayer player) : base(player) { }

    public override void Enter()
    {
        Vector2 origin = player.transform.position;
        Vector2 direction = player.transform.right;
        player.interaction.Interact(origin, direction);

        // 상호작용은 즉시 완료되므로 바로 이전 상태로 돌아갑니다
        if (player.inputVec.x != 0)
            player.ChangeState(PlayerState.Run);
        else
            player.ChangeState(PlayerState.Idle);
    }
}
