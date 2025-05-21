using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClimbState : PlayerStateMachine
{
    public float climbSpeed = 5f;
    public float originalGravity;

    public ClimbState(ControllerPlayer player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
        player.isOnLadder = true;
        player.animator.SetTrigger("LadderTrigger");
        player.animator.SetBool("IsLadder", true);
        Debug.Log("트리거 진입");

        player.currentJumpCount = 0;

        player.playerCollider.excludeLayers = player.platformLayerMask;

        // 수평 속도 0으로 설정
        player.rigid.velocity = new Vector2(0, 0);

        originalGravity = player.rigid.gravityScale;
        player.rigid.gravityScale = 0;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        // Y축 입력에 따라 상하 이동
        player.rigid.velocity = new Vector2(player.inputVec.x * 0.5f, player.inputVec.y) * climbSpeed;

        if (Mathf.Abs(player.inputVec.y) > 0.1f)
        {
            if (!player.animator.GetBool("IsMovingLadder"))
            {
                player.animator.SetBool("IsMovingLadder", true);
            }
        }
        else
        {
            if (player.animator.GetBool("IsMovingLadder"))
            {
                player.animator.SetBool("IsMovingLadder", false);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.isOnLadder = false;
        player.animator.SetBool("IsLadder", false);
        player.rigid.gravityScale = originalGravity;

        player.rigid.velocity = Vector2.zero;
        player.playerCollider.excludeLayers = 0;
    }

    public override void OnJump()
    {
        base.OnJump();
        if (player.CanJump)
            player.ChangeState(PlayerState.Jump);
    }

    public override void OnCollisionExit(Collision2D collision)
    {
        base.OnCollisionExit(collision);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Climb"))
        {
            // 원래 상태로 돌아가기
            player.ChangeState(PlayerState.Idle);
        }
    }

    public override void OnTriggerExit(Collider2D collision)
    {
        base.OnTriggerExit(collision);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Climb"))
        {
            // 원래 상태로 돌아가기
            player.ChangeState(PlayerState.Idle);
        }
    }
}
