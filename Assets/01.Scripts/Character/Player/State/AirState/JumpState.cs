using UnityEngine;

public class JumpState : AirState
{
    public JumpState(ControllerPlayer player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
        // 첫 점프인지 더블 점프인지 확인
        if (player.currentJumpCount < 2)
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

        AnimatorStateInfo stateInfo = player.animator.GetCurrentAnimatorStateInfo(0);

        if (player.rigid.velocity.y < -player.rigid.gravityScale * Time.fixedDeltaTime && stateInfo.IsTag("Attack"))
            player.playerCollider.excludeLayers = 0;

        if (player.rigid.velocity.y < -player.rigid.gravityScale * Time.fixedDeltaTime && !stateInfo.IsTag("Attack") && !player.IsJumpAttacking())
            player.ChangeState(PlayerState.Fall);
    }

    public override void OnCollisionEnter(Collision2D collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.CompareTag("Ground"))
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
        player.animator.SetBool("IsJump", false);
        player.playerCollider.excludeLayers = 0;
    }
}
