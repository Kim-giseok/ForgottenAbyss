using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : AirState
{
    public JumpState(ControllerPlayer player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
        // 첫 점프인지 더블 점프인지 확인
        if (player.currentJumpCount == 0)
        {
            // 첫 번째 점프
            player.animator.SetBool("IsJump", true);
            player.isGround = false;
        }
        player.currentJumpCount++;
        playerSound.JumpSound();

        // 공통 점프 로직
        player.rigid.velocity = new Vector2(player.rigid.velocity.x, 0); // y축 속도 초기화
        player.rigid.AddForce(Vector2.up * player.jumpPower, ForceMode2D.Impulse);

        player.playerCollider.excludeLayers = player.platformLayerMask;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (player.rigid.velocity.y < -player.rigid.gravityScale * Time.fixedDeltaTime)
            player.ChangeState(PlayerState.Fall);
    }

    public override void OnJump()
    {
        if (player.inputVec.y < 0)
            player.ChangeState(PlayerState.Drop);
        else
            base.OnJump();
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.CompareTag("Ground"))
            player.ChangeState(PlayerState.Idle);
    }

    public override void Exit()
    {
        base.Exit();
        player.animator.SetBool("IsJump", false);
        player.playerCollider.excludeLayers = 0;
    }
}
