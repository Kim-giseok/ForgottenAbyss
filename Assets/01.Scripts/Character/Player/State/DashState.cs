using System.Collections;
using UnityEngine;

public class DashState : PlayerStateMachine
{
    private float dashTimer = 0f;
    public DashState(ControllerPlayer player) : base(player) { }
    public override void Enter()
    {
        base.Enter();
        player.status.SetStat(StatType.CurrentMP, player.status.stats[StatType.CurrentMP] - player.dashCost);

        playerSound.DashSound();
        player.animator.SetBool("IsDash", true);
        player.animator.SetTrigger("DashTrigger");
        player.SetInvincibility(player.dashTime + 0.5f);

        player.canAttack = false;
        player.canSkill = false;

        dashTimer = 0f;

        player.rigid.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        Vector2 dashDirection = new Vector2(player.inputVec.x, 0);
        player.rigid.velocity = new Vector2(dashDirection.x * player.dashDistance / player.dashTime, player.rigid.velocity.y);
        //Debug.Log($"{player.dashDistance/player.dashTime}");
    }

    public override void Update()
    {
        base.Update();
        dashTimer += Time.deltaTime;
        if (dashTimer >= player.dashTime) //대쉬 종료
        {
            player.ChangeState(PlayerState.Idle);

            player.rigid.velocity = new Vector2(player.inputVec.x * player.status.stats[StatType.SPEED], player.rigid.velocity.y);
        }
    }

    public override void OnJump()
    {
        base.OnJump();
        if (player.CanJump)
            player.ChangeState(PlayerState.Jump);
    }

    public override void Exit()
    {
        base.Exit();
        player.animator.SetBool("IsDash", false);
        SkillController.Instance.ResetAttack();

        player.canAttack = true;
        player.canSkill = true;

        player.StartCoroutine(WaitForLandingToResetCollision());
        player.rigid.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        player.StartCoroutine(DashCooldown(0.5f));
    }

    private IEnumerator WaitForLandingToResetCollision()
    {
        yield return new WaitUntil(() => player.isGround);
        Debug.Log("대쉬 종료 후 착지 시 충돌 복구됨!");
    }

    private IEnumerator DashCooldown(float delay)
    {
        player.canDash = false;
        yield return new WaitForSeconds(delay);
        player.canDash = true;
    }
}
