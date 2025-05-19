using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : AirState
{
    public FallState(ControllerPlayer player) : base(player)
    {
    }
    public override void Enter()
    {
        base.Enter();

        player.animator.SetTrigger("FallTrigger");
        player.animator.SetBool("IsFall", true);
    }

    public override void Update()
    {
        base.Update();
        if (player.isGround)
        {
            AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);
            bool isJumpAttack = stateInfo.IsTag("Attack") && stateInfo.normalizedTime < 1f;

            if (isJumpAttack)
                player.WaitForEnd();
            else
                player.ChangeState(PlayerState.Idle);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.animator.SetBool("IsFall", false);
    }
}
