using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClimbState : PlayerStateMachine
{
    public float climbSpeed = 5f;
    public ClimbState(ControllerPlayer player) : base(player) { }

    public override void Enter()
    {
        //중력 무시(선택적)
        //player.rigid.gravityScale = 1;

        // 수평 속도 0으로 설정
        player.rigid.velocity = new Vector2(0, player.rigid.velocity.y);
    }

    public override void FixedUpdate()
    {
        //// Y축 입력에 따라 상하 이동
        player.rigid.velocity = new Vector2(0, player.inputVec.y * climbSpeed);
        
    }
    public override void Update()
    {
        // 벽에서 떨어졌는지 체크
        if (!player.isWallDetected)
        {
            player.ChangeState(PlayerState.Idle);
            player.rigid.velocity = new Vector2(0, 0);
            player.transform.position += new Vector3(-1, 1, 0);
            return;
        }

        // 점프 키 입력 시 벽에서 튕겨나가는 점프 가능
    }
    public override void OnJump()
    {
        // 벽에서 반대 방향으로 점프
        Vector2 jumpDirection = new Vector2(-player.transform.right.x * 15f, 12f); // 반대 방향으로 더 강하게

        player.rigid.velocity = Vector2.zero;
        player.rigid.AddForce(jumpDirection.normalized * 100f, ForceMode2D.Impulse);
        player.isWallDetected = false;



        player.ChangeState(PlayerState.Jump);

    }
}
