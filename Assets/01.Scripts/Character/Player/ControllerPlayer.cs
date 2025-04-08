using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerPlayer : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed; //이동속도
    public float jumpPower; //점프력
    public float dashDistance; //대쉬거리
    public float dashTime; //대쉬지속시간
    public LayerMask platformLayerMask; //점프 중 무시할 플랫폼 레이어

    private bool isGround; //땅 밟고 있는지 여부
    private bool isDashing = false; //대쉬 여부
    private bool isAttacking = false; //공격 여부
    private bool isIgnoringCollision = false; //콜라이더 충돌 무시 여부
    

    Rigidbody2D rigid;
    Animator animator;
    Collider2D playerCollider;

    PlayerInteraction interaction;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        interaction = GetComponent<PlayerInteraction>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (!isDashing && !isAttacking)
        {
            rigid.velocity = new Vector2(inputVec.x * speed, rigid.velocity.y);
            UpdateDirection();
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
        animator.SetBool("IsRun", inputVec.x != 0);
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && isGround && !isAttacking) //땅에 닿아 있을 때 점프 가능
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isGround = false;
            animator.SetBool("IsJump", true);

            StartCoroutine(IgnorePlatformCollision(true));
            StartCoroutine(ResetIgnoreCollision(0.4f));
            
        }
    }

    void OnDash(InputValue value) //대쉬 키 입력
    {
        if (value.isPressed && !isDashing && isGround && !isAttacking)
        {
            StartCoroutine(Dash());
        }
    }

    public void OnAttack(InputValue value) //일반공격 키 입력
    {
        if (value.isPressed && !isDashing && !isAttacking )
        {
            StartCoroutine (Attack());
        }
    }

    void OnFirstSkill() //1번스킬 키 입력
    {
        Debug.Log("S: 스킬1");
    }

    void OnSecondSkill() //2번스킬 키 입력
    {
        Debug.Log("D: 스킬2");
    }

    void OnSpecialSkill() //특수스킬 키 입력
    {
        Debug.Log("R: 특수 스킬");
    }

    void OnInteraction() //상호 작용 키 입력
    {
        Vector2 origin = transform.position;
        Vector2 direction = transform.right;
        interaction.Interact(origin, direction);
        Debug.Log("F: 상호작용");
    }

    void OnInventory() //인벤토리 키 입력
    {
        Debug.Log("I: 인벤토리 열기");
    }

    void OnMenu() //메뉴 키 입력
    {
        Debug.Log("Esc: 메뉴창 열기");
    }

    void OnOtherWeapon() //무기변환 키 입력
    {
        Debug.Log("Z: 다른무기로 변환");
    }

    void UpdateDirection() //방향 전환
    {
        if(inputVec.x < 0)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else if(inputVec.x > 0)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            animator.SetBool("IsJump", false);
        }

        
    }

    IEnumerator Dash()
    {
        isDashing = true; //대쉬 시작
        
        Vector2 dashDirection = new Vector2(inputVec.x, 0); //현재 이동 방향
        rigid.velocity = new Vector2(dashDirection.x * dashDistance / dashTime, rigid.velocity.y);


        yield return new WaitForSeconds(dashTime);
        
        isDashing = false; //대쉬 종료
        rigid.velocity = new Vector2(inputVec.x * speed, rigid.velocity.y); //원래 속도로 복귀
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        animator.SetBool("IsRun", false);
        rigid.velocity = new Vector2(0, rigid.velocity.y);

        yield return new WaitForSeconds(0.4f);

        ProjectileManager.Instance.CreateProjectile(transform, 10);

        yield return new WaitForSeconds(0.1f);
              
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
        ProjectileManager.Instance.DestroyProjectile(transform);

        // 공격 종료 후 방향키가 여전히 눌려있다면 속도 복원
        if (inputVec.x != 0)
        {
            animator.SetBool("IsRun", true);
            rigid.velocity = new Vector2(inputVec.x * speed, rigid.velocity.y);
        }
    }

    IEnumerator IgnorePlatformCollision(bool ignore) //플랫폼 콜라이더 무시
    {
        isIgnoringCollision = ignore;

        //플랫폼 레이어의 모든 콜라이더 찾기
        Collider2D[] platformColliders = Physics2D.OverlapCircleAll(transform.position, 10f, platformLayerMask);

        foreach (Collider2D platformCollider in platformColliders) //콜라이더 무시
        {
            if(platformCollider != null && platformCollider.CompareTag("Ground"))
            {
                Physics2D.IgnoreCollision(playerCollider, platformCollider, ignore);
            }
        }

        yield return null;
    }

    IEnumerator ResetIgnoreCollision(float delay) //콜라이더 무시 상태 초기화
    {
        yield return new WaitForSeconds(delay);

        if (!isGround) //땅에 닿아있지 않을 때
        {
            StartCoroutine(IgnorePlatformCollision(false)); 
            isIgnoringCollision = false;
        }
    }
}
