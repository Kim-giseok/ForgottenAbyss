using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideState : PlayerStateMachine
{
    public SlideState(ControllerPlayer player) : base(player) { }

    public float originalGravity;
    public float originaljumpPower;

    public override void Enter()
    {
        Debug.Log("Slide");

        player.animator.SetBool("IsWall", true);

        originalGravity = player.rigid.gravityScale;

        originaljumpPower = player.jumpPower;

        player.rigid.velocity = new Vector2(0, player.rigid.velocity.y);

        player.rigid.gravityScale = 0.1f;

        //player.jumpPower = 100f;

    }

    public override void Update()
    {
        //if (player.isGround && Mathf.Abs(player.rigid.velocity.x) < 0.1f)
        //{
        //    player.ChangeState(PlayerState.Idle);
        //}
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

    public override void OnCollisionEnter(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Water"))
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
