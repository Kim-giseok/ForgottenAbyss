using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : AirState
{
    public JumpState(ControllerPlayer player) : base(player) { }

    private bool isFastFalling = false;
    private float fastFallSpeed = 15f;
    public override void Enter()
    {
        // 첫 점프인지 더블 점프인지 확인
        if (player.currentJumpCount == 0)
        {
            // 첫 번째 점프
            player.animator.SetBool("IsJump", true);
            player.isGround = false;
            player.currentJumpCount = 1;
            playerSound.JumpSound();

        }
        else
        {
            // 더블 점프 (또는 추가 점프)
            player.currentJumpCount++;
            playerSound.JumpSound();

        }

        // 공통 점프 로직
        player.rigid.velocity = new Vector2(player.rigid.velocity.x, 0); // y축 속도 초기화
        player.rigid.AddForce(Vector2.up * player.jumpPower, ForceMode2D.Impulse);

        player.playerCollider.excludeLayers = player.platformLayerMask;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (isFastFalling)
        {
            player.rigid.velocity = new Vector2(0, -fastFallSpeed);
        }
        player.UpdateDirection();

        if (player.rigid.velocity.y < -player.rigid.gravityScale * Time.fixedDeltaTime)
            player.ChangeState(PlayerState.Fall);
    }

    public override void OnJump()
    {
        if (player.inputVec.y < 0)
        {
            ActivateFastFall();
            return;
        }
        base.OnJump();
    }

    public override void OnDash()
    {
        if (!isFastFalling) // 빠른 낙하 중이 아닐 때만 대쉬 가능
        {
            player.animator.SetBool("IsJump", false);
            player.ChangeState(PlayerState.Dash);
        }
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (player.inputVec.x != 0)
            {
                player.ChangeState(PlayerState.Run);
            }
            else
            {
                player.ChangeState(PlayerState.Idle);
            }
        }
    }

    private void ActivateFastFall()
    {
        if (!isFastFalling)
        {
            isFastFalling = true;
                       
            // 플레이어가 움직이지 않도록 수평 속도를 0으로 설정
            player.rigid.velocity = new Vector2(0, -fastFallSpeed);
        }
    }

    public override void Exit()
    {
        isFastFalling = false; // 상태 종료 시 빠른 낙하 상태 초기화
        player.playerCollider.excludeLayers = 0;
    }
}
