using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerStateMachine
{
    public JumpState(ControllerPlayer player) : base(player) { }
    public override void Enter()
    {
        // 첫 점프인지 더블 점프인지 확인
        if (player.currentJumpCount == 0)
        {
            // 첫 번째 점프
            player.animator.SetBool("IsJump", true);
            player.isGround = false;
            player.currentJumpCount = 1;

            // 플랫폼 콜라이더 무시 설정
            player.StartCoroutine(player.IgnorePlatformCollision(true));
            player.StartCoroutine(player.ResetIgnoreCollision(0.5f));
        }
        else
        {
            // 더블 점프 (또는 추가 점프)
            player.currentJumpCount++;
            
        }

        // 공통 점프 로직
        player.rigid.velocity = new Vector2(player.rigid.velocity.x, 0); // y축 속도 초기화
        player.rigid.AddForce(Vector2.up * player.jumpPower, ForceMode2D.Impulse);

    }
    public override void FixedUpdate()
    {
        player.rigid.velocity = new Vector2(player.inputVec.x * player.speed, player.rigid.velocity.y);
        player.UpdateDirection();
    }
    public override void OnJump()
    {
        // 추가 점프가 가능하면 다시 자기 자신의 Enter() 호출
        if (player.currentJumpCount < player.jumplimit)
        {
            // 현재 상태를 유지하면서 Enter() 메서드만 다시 호출
            Enter();

        }
    }
    public override void OnAttack()
    {
        //player.ChangeState(PlayerState.Attack);
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
    public override void Update()
    {
        // 벽 감지 및 입력 조건 확인
        if (player.isWallDetected )
        {
            
            player.ChangeState(PlayerState.Climb);
            Debug.Log("Climb");
        }
    }
}
