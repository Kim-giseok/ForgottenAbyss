using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : PlayerStateMachine
{
    private float dashTimer = 0f;
    public DashState(ControllerPlayer player) : base(player) { }
    public override void Enter()
    {
        player.animator.SetBool("IsDash", true);
        player.animator.SetTrigger("DashTrigger");
        player.SetInvincibility(true);
        dashTimer = 0f;

        player.rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        Vector2 dashDirection = new Vector2(player.inputVec.x, 0);
        player.rigid.velocity = new Vector2(dashDirection.x * player.dashDistance / player.dashTime, player.rigid.velocity.y);
        Debug.Log($"{player.dashDistance/player.dashTime}");
    }
    public override void Update()
    {
        dashTimer += Time.deltaTime;
        if (dashTimer >= player.dashTime) //대쉬 종료
        {
            if (player.inputVec.x != 0)
                player.ChangeState(PlayerState.Run);
            else
                player.ChangeState(PlayerState.Idle);

            player.rigid.velocity = new Vector2(player.inputVec.x * player.speed, player.rigid.velocity.y);
        }
    }
    public override void Exit()
    {
        player.animator.SetBool("IsDash", false);
        SkillController.Instance.ResetAttack();

        player.rigid.collisionDetectionMode = CollisionDetectionMode2D.Discrete;

        player.StartCoroutine(DelayedInvincibilityOff(0.25f)); //대쉬 무적판정 조금 더 길게
    }

    private IEnumerator DelayedInvincibilityOff(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.SetInvincibility(false);
    }
}
