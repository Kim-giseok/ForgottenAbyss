using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class SlideState : PlayerStateMachine
{
    public SlideState(ControllerPlayer player) : base(player) { }

    public float originalGravity;
    public float originaljumpPower;
    public float wallCheckDistance = 0.5f;

    public override void Enter()
    {
        Debug.Log("Slide");

        player.animator.SetBool("IsWall", true);

        originalGravity = player.rigid.gravityScale;

        originaljumpPower = player.jumpPower;

        player.rigid.velocity = new Vector2(0, player.rigid.velocity.y);

        player.rigid.gravityScale = 0.5f;

        //player.jumpPower = 100f;

        ////벽 꼭대기 확인
        //if (IsAtWallTop())
        //{
        //    // 꼭대기에 있으면 슬라이드 상태로 진입하지 않고 바로 Idle 상태로
        //    player.ChangeState(PlayerState.Idle);
        //    return;
        //}

    }

    public override void Update()
    {
        Vector2 headPosition = new Vector2(player.transform.position.x,
            player.transform.position.y + player.playerCollider.bounds.extents.y);

        Vector2 Direction = player.transform.right;

        Debug.DrawRay(headPosition - new Vector2(0, 0.1f), Direction * wallCheckDistance, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(headPosition, Direction, wallCheckDistance, 1 << LayerMask.NameToLayer("Wall"));

        //if(hit.collider != null)
        //{
        //    Debug.Log("슬라이딩");
        //}
        if (hit.collider == null)
        {
            if (player.inputVec.x == 0)
            {
                player.ChangeState(PlayerState.Idle);
            }
            else
            {
                player.ChangeState(PlayerState.Run);
            }
            return;
        }
    }

    public override void Exit()
    {
        player.animator.SetBool("IsWall", false);
        player.rigid.gravityScale = originalGravity;
        player.jumpPower = originaljumpPower;
    }

    public override void OnJump()
    {
        player.ChangeState(PlayerState.Jump);
        Debug.Log("Space");
    }

    //private bool IsAtWallTop(Vector2 origin, Vector2 direction)
    //{
    //    // 플레이어 머리 위치 계산
    //    Vector2 headPosition = new Vector2(player.transform.position.x,
    //        player.transform.position.y + player.playerCollider.bounds.extents.y);

    //    Vector2 Direction = player.transform.right;
        
    //    Debug.DrawRay(headPosition - new Vector2(0, 0.1f), Direction * wallCheckDistance, Color.red);

    //    RaycastHit2D hit = Physics2D.Raycast(origin, direction, wallCheckDistance, LayerMask.NameToLayer("Wall"));

    //    if(hit.collider == null)
    //    {
    //        return false;
    //    }

    //}

    public override void OnCollisionEnter(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            player.ChangeState(PlayerState.Idle);
        }
    }

    public override void OnCollisionExit(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            player.ChangeState(PlayerState.Idle);
        }
    }
}
