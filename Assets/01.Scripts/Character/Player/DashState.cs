using System.Collections;
using UnityEngine;

public class DashState : PlayerStateMachine
{
    private float dashTimer = 0f;
    public DashState(ControllerPlayer player) : base(player) { }
    public override void Enter()
    {
        PlayerStatus status = GameManager.Instance.player.playerstatus;
        float curMp = status.stats[StatType.CurrentMP];
        float cost = 10f;

        if (curMp < cost)
        {
            Debug.Log("ìŠ¤íƒœë¯¸ë„ˆ ë¶€ì¡± - ëŒ€ì‰¬ ì·¨ì†Œ");

            player.ChangeState(player.previousState);
            return;
        }

        status.SetStat(StatType.CurrentMP, curMp - cost);

        playerSound.DashSound();
        player.animator.SetBool("IsDash", true);
        player.animator.SetTrigger("DashTrigger");
        player.SetInvincibility(true);

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
        // ´ë½¬ Áß Á¡ÇÁÅ° ÀÔ·Â °¨Áö
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Á¡ÇÁ »óÅÂ·Î Áï½Ã ÀüÈ¯
            player.ChangeState(PlayerState.Jump);
            return; // Á¡ÇÁ »óÅÂ·Î ÀüÈ¯ÇßÀ¸¹Ç·Î ³ª¸ÓÁö Update ·ÎÁ÷ °Ç³Ê¶Ü
        }

        dashTimer += Time.deltaTime;
        if (dashTimer >= player.dashTime) //ëŒ€ì‰¬ ì¢…ë£Œ
        {
            if (player.inputVec.x != 0 && player.isGround)
                player.ChangeState(PlayerState.Run);
            else if(player.inputVec.x == 0  && player.isGround)
                player.ChangeState(PlayerState.Idle);

            player.rigid.velocity = new Vector2(player.inputVec.x * player.status.stats[StatType.SPEED], player.rigid.velocity.y);
        }
    }
    public override void Exit()
    {
        player.animator.SetBool("IsDash", false);
        SkillController.Instance.ResetAttack();

        player.canAttack = true;
        player.canSkill = true;

        player.StartCoroutine(WaitForLandingToResetCollision());
        player.rigid.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        player.StartCoroutine(DelayedInvincibilityOff(0.25f)); //ëŒ€ì‰¬ ë¬´ì íŒì • ì¡°ê¸ˆ ë” ê¸¸ê²Œ
    }

    private IEnumerator DelayedInvincibilityOff(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.SetInvincibility(false);
    }

    private IEnumerator WaitForLandingToResetCollision()
    {
        yield return new WaitUntil(() => player.isGround);
        player.IgnorePlatformCollision();
        Debug.Log("ëŒ€ì‰¬ ì¢…ë£Œ í›„ ì°©ì§€ ì‹œ ì¶©ëŒ ë³µêµ¬ë¨!");
    }
}
