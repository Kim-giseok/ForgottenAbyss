using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClimbState : PlayerStateMachine
{
    public float climbSpeed = 5f;
    public float originalGravity;
    public bool isMoving;
    public ClimbState(ControllerPlayer player) : base(player) { }

    public override void Enter()
    {
        player.isOnLadder = true;
        player.animator.SetTrigger("LadderTrigger");
        player.animator.SetBool("IsLadder", true);
        Debug.Log("트리거 진입");
        originalGravity = player.rigid.gravityScale;

        Physics2D.IgnoreLayerCollision(player.gameObject.layer, LayerMask.NameToLayer("Ground"), true);

        // 현재 접촉 중인 WallClimb 레이어의 트리거 찾기
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, 1f);
        Collider2D Collider = null;

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Climb"))
            {
                Collider = collider;
                break;
            }
        }

        // 트리거가 있다면 플레이어를 중앙으로 이동
        if (Collider != null)
        {
            // 트리거의 중앙 X 위치 계산
            Vector2 centerPosition = Collider.bounds.center;

            // 플레이어의 X 위치만 트리거의 중앙으로 변경
            Vector3 newPosition = player.transform.position;
            newPosition.x = centerPosition.x;
            player.transform.position = newPosition;
        }

        // 수평 속도 0으로 설정
        player.rigid.velocity = new Vector2(0, 0);
        player.rigid.gravityScale = 0;
    }

    public override void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, 1f);
        Collider2D Collider = null;

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Climb"))
            {
                Collider = collider;
                break;
            }
        }

        if (player.inputVec.y > 0 && player.transform.position.y >= Collider.bounds.max.y - 1.1f)
        {
                       
            Vector3 topPosition = new Vector3(Collider.bounds.center.x,
                Collider.bounds.max.y + player.playerCollider.bounds.extents.y - 0.4f, 0 );

            player.transform.position = topPosition;
            player.ChangeState(PlayerState.Idle);
            return;
        }
    }
    public override void FixedUpdate()
    {
        // Y축 입력에 따라 상하 이동
        player.rigid.velocity = new Vector2(0, player.inputVec.y * climbSpeed);
    }

    public override void Exit()
    {
        player.isOnLadder = false;
        player.animator.SetBool("IsLadder", false);
        player.rigid.gravityScale = originalGravity;

        Physics2D.IgnoreLayerCollision(player.gameObject.layer, LayerMask.NameToLayer("Ground"), false);
    }

    public override void OnJump()
    {
        player.ChangeState(PlayerState.Idle);
    }
    public override void OnCollisionExit(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Climb"))
        {
            // 원래 상태로 돌아가기
            player.ChangeState(PlayerState.Idle);
        }

    }
    public override void OnTriggerExit(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Climb"))
        {
            // 원래 상태로 돌아가기
            player.ChangeState(PlayerState.Idle);
        }
    }

}
    

