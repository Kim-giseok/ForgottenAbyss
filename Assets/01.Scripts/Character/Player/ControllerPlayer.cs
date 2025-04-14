using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;

public class ControllerPlayer : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed; //이동속도
    public float jumpPower; //점프력
    public float dashDistance; //대쉬거리
    public float dashTime; //대쉬지속시간
    public int jumplimit; //점프 가능 횟수
    public int currentJumpCount; //현재 점프 횟수
    public LayerMask platformLayerMask; //점프 중 무시할 플랫폼 레이어
    public LayerMask invincibilityLayerMask; //무적 상태에서 무시할 레이어

    //public LayerMask wallLayer; //벽 감지 레이어
    //public float wallDistance; //벽 감지 거리

    //public bool isWallDetected; //벽 감지 여부
    //public bool isWallClimbing; //등반 가능한 벽인지 여부
    //public RaycastHit2D wallHit;

    public bool isGround; //땅 밟고 있는지 여부
    public bool isDashing = false; //대쉬 여부
    public bool isAttacking = false; //공격 여부
    //public bool isIgnoringCollision = false; //콜라이더 충돌 무시 여부
    public bool isInvincible = false; //무적 상태 여부

    public Rigidbody2D rigid;
    public Animator animator;
    public Collider2D playerCollider;
    public SpriteRenderer spriteRenderer;

    public PlayerInteraction interaction;

    // FSM 관련 변수
    private Dictionary<PlayerState, PlayerStateMachine> states = new Dictionary<PlayerState, PlayerStateMachine>();
    private PlayerState currentState;

    private bool isFacingRight = true;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        interaction = GetComponent<PlayerInteraction>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // 상태 머신 초기화
        InitStateMachine();
    }

    private void InitStateMachine()
    {
        // 상태 등록
        states.Add(PlayerState.Idle, new IdleState(this));
        states.Add(PlayerState.Run, new RunState(this));
        states.Add(PlayerState.Jump, new JumpState(this));
        states.Add(PlayerState.Dash, new DashState(this));
        //states.Add(PlayerState.Attack, new AttackState(this));
        states.Add(PlayerState.Interaction, new InteractionState(this));
        states.Add(PlayerState.Climb, new ClimbState(this));
        states.Add(PlayerState.Slide, new SlideState(this));

        // 초기 상태 설정
        if (currentState == 0 || !states.ContainsKey(currentState))
        {
            ChangeState(PlayerState.Idle);
        }
    }

    public void ChangeState(PlayerState newState)
    {
        // 현재 상태가 있다면 Exit 호출
        if (states.ContainsKey(currentState))
        {
            states[currentState].Exit();
        }

        // 상태 변경
        currentState = newState;

        // 새 상태의 Enter 호출
        if (states.ContainsKey(currentState))
        {
            states[currentState].Enter();
        }
    }

    private void Update()
    {
        // 현재 상태 업데이트
        if (states.ContainsKey(currentState))
        {
            states[currentState].Update();
            //Debug.Log($"{states[currentState]}");
        }


        if (!isGround && rigid.velocity.y < -0.1f)
        {
            animator.SetBool("IsFall", true);
        }
        else
        {
            animator.SetBool("IsFall", false);
        }

        //CheckWall();
    }
    private void FixedUpdate()
    {
        // 현재 상태 FixedUpdate
        if (states.ContainsKey(currentState))
        {
            states[currentState].FixedUpdate();
        }

        //if (!isDashing && !isAttacking)
        //{
        //    rigid.velocity = new Vector2(inputVec.x * speed, rigid.velocity.y);
        //    UpdateDirection();
        //}
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();

        if (states.ContainsKey(currentState))
        {
            states[currentState].OnMove(inputVec);
        }
    }

    //void OnJump(InputValue value)
    //{
    //    if (value.isPressed && !isAttacking && currentJumpCount < jumplimit) //점프 가능 조건
    //        if (isGround)
    //        {
    //            rigid.velocity = new Vector2(rigid.velocity.x, 0); // y축 속도 초기화
    //            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    //            isGround = false;
    //            animator.SetBool("IsJump", true);
    //            currentJumpCount = 1;

    //            StartCoroutine(IgnorePlatformCollision(true));
    //            StartCoroutine(ResetIgnoreCollision(0.5f));
    //        }
    //        else if (currentJumpCount < jumplimit)
    //        {
    //            rigid.velocity = new Vector2(rigid.velocity.x, 0); // y축 속도 초기화
    //            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    //            currentJumpCount++;
    //        }
    //}
    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            // 현재 상태에 점프 입력 전달
            if (states.ContainsKey(currentState))
            {
                states[currentState].OnJump();
            }
        }
    }

    void OnDash(InputValue value) //대쉬 키 입력
    {
        //if (value.isPressed && !isDashing && isGround && !isAttacking && inputVec.x != 0)
        //{
        //    StartCoroutine(Dash());
        //}
        if (value.isPressed)
        {
            // 현재 상태에 대시 입력 전달
            if (states.ContainsKey(currentState))
            {
                states[currentState].OnDash();
            }
        }
    }

    //public void OnAttack(InputValue value) //일반공격 키 입력
    //{
    //    if (value.isPressed && !isDashing && !isAttacking )
    //    {
    //        StartCoroutine (Attack());
    //    }
    //}

    //void OnFirstSkill() //1번스킬 키 입력
    //{
    //    Debug.Log("S: 스킬1");
    //}

    //void OnSecondSkill() //2번스킬 키 입력
    //{
    //    Debug.Log("D: 스킬2");
    //}

    //void OnSpecialSkill() //특수스킬 키 입력
    //{
    //    Debug.Log("R: 특수 스킬");
    //}

    void OnInteraction() //상호 작용 키 입력
    {
        //Vector2 origin = transform.position;
        //Vector2 direction = transform.right;
        //interaction.Interact(origin, direction);
        if (states.ContainsKey(currentState))
        {
            states[currentState].OnInteraction();
        }
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
        WeaponManager.Instance.SwapWeapon();
        Debug.Log("Z: 다른무기로 변환");
    }

    public void UpdateDirection() //방향 전환
    {
        if (inputVec.x < 0 && isFacingRight)
        {
            // 오른쪽 → 왼쪽으로 바뀜
            isFacingRight = false;
            transform.localEulerAngles = new Vector3(0, 180, 0);
            animator.SetTrigger("TurnTrigger");
        }
        else if (inputVec.x > 0 && !isFacingRight)
        {
            // 왼쪽 → 오른쪽으로 바뀜
            isFacingRight = true;
            transform.localEulerAngles = new Vector3(0, 0, 0);
            animator.SetTrigger("TurnTrigger");
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //// 무적 상태일 때는 충돌 무시
        //if (isInvincible && ((1 << collision.gameObject.layer) & invincibilityLayerMask) != 0)
        //{
        //    Debug.Log("1");
        //    // 적이나 투사체 등과의 충돌 무시
        //    Physics2D.IgnoreCollision(playerCollider, collision.collider, true);
        //    return;
        //}

        //if (collision.gameObject.CompareTag("Ground"))
        //{
        //    isGround = true;
        //    animator.SetBool("IsJump", false);
        //    currentJumpCount = 0;

        //    if (isIgnoringCollision) //땅에 닿으면 무시 상태 해제
        //    {
        //        StartCoroutine(IgnorePlatformCollision(false));
        //        isIgnoringCollision = false;
        //    }
        //}
        //else if (isIgnoringCollision) //다른 물체와 충돌 시 무시 상태 해제
        //{
        //    StartCoroutine(IgnorePlatformCollision(false));
        //    isIgnoringCollision = false;
        //}

        // 현재 상태에 충돌 이벤트 전달
        if (states.ContainsKey(currentState))
        {
            states[currentState].OnCollisionEnter(collision);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
            animator.SetBool("IsJump", false);
            currentJumpCount = 0;
            rigid.velocity = Vector3.zero;

            //if (isIgnoringCollision)
            //{
            //    StartCoroutine(IgnorePlatformCollision(false));
            //    isIgnoringCollision = false;
            //}
        }
        //else if (isIgnoringCollision)
        //{
        //StartCoroutine(IgnorePlatformCollision(false));
        //isIgnoringCollision = false;
        //}
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Debug.Log("1");
            ChangeState(PlayerState.Slide);
        }

    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        //{
        //    ChangeState(PlayerState.Slide);
        //}
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        // 현재 상태에 충돌 종료 이벤트 전달
        if (states.ContainsKey(currentState))
        {
            states[currentState].OnCollisionExit(collision);
        }
        
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Climb") && inputVec.y > 0)
        {
            ChangeState(PlayerState.Climb);
        }

        if (states.ContainsKey(currentState))
        {
            states[currentState].OnTriggerStay(collision);
        }

    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (states.ContainsKey(currentState))
        {
            states[currentState].OnTriggerExit(collision);
        }

    }

    public void SetInvincibility(bool isInvincible)
    {
        this.isInvincible = isInvincible;

        // 무적 상태 투명도 조절
        if (isInvincible)
        {
            StartCoroutine(InvincibleEffect());

            Collider2D[] Colliders = Physics2D.OverlapCircleAll(transform.position, 10f, invincibilityLayerMask);

            foreach (Collider2D Collider in Colliders) //콜라이더 무시
            {
                if (Collider != null )
                {
                    Physics2D.IgnoreCollision(playerCollider, Collider, true);
                }
            }
        }
        else
        {
            // 무적 해제 시 원래 상태로 복귀
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            ResetIgnoredCollision(); //무적 상태 해제
        }

        //if (!isInvincible)
        //{
        //    ResetIgnoredCollision(); //무적 상태 해제
        //}

    }

    public void ResetIgnoredCollision() //무적 상태 해제
    {
        // 무적 중에 무시했던 모든 콜라이더와의 충돌 설정 초기화
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f, invincibilityLayerMask);

        foreach (Collider2D collider in colliders)
        {
            Physics2D.IgnoreCollision(playerCollider, collider, false);
        }
    }

    //public void CheckWall()
    //{
    //    Vector2 direction = transform.right;
    //    wallHit = Physics2D.Raycast(transform.position, direction, wallDistance, wallLayer);

    //    Debug.DrawRay(transform.position, direction * wallDistance, Color.green);

    //    if(isWallDetected = wallHit.collider != null)
    //    {
    //        Debug.Log("벽감지"); 
    //    }
    //}

    //public IEnumerator Dash()
    //{
    //    isDashing = true; //대쉬 시작
    //    SetInvincibility(true); //무적 상태 시작

    //    Vector2 dashDirection = new Vector2(inputVec.x, 0); //현재 이동 방향
    //    rigid.velocity = new Vector2(dashDirection.x * dashDistance / dashTime, rigid.velocity.y);


    //    yield return new WaitForSeconds(dashTime);

    //    isDashing = false; //대쉬 종료
    //    SetInvincibility(false); //무적 상태 종료

    //    rigid.velocity = new Vector2(inputVec.x * speed, rigid.velocity.y); //원래 속도로 복귀
    //}

    public IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        animator.SetBool("IsRun", false);
        rigid.velocity = new Vector2(0, rigid.velocity.y);

        yield return new WaitForSeconds(0.35f);

        ProjectileManager.Instance.CreateMeleeProjectile(transform, 10);

        yield return new WaitForSeconds(0.1f);
              
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
        ProjectileManager.Instance.DestroyMeleeProjectile(transform);

        // 공격 종료 후 방향키가 여전히 눌려있다면 속도 복원
        if (inputVec.x != 0)
        {
            animator.SetBool("IsRun", true);
            rigid.velocity = new Vector2(inputVec.x * speed, rigid.velocity.y);
        }
    }

    public void IgnorePlatformCollision()
    {
        Collider2D[] platformColliders = Physics2D.OverlapCircleAll(transform.position, 10f, platformLayerMask);

        foreach (Collider2D platformCollider in platformColliders) //콜라이더 무시
        {
            if (rigid.velocity.y < 0)
            {
                Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
            }
            else
            {
                Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
            }
        }
    }

    //public IEnumerator IgnorePlatformCollision(bool ignore) //플랫폼 콜라이더 무시
    //{
    //    isIgnoringCollision = ignore;

    //    //플랫폼 레이어의 모든 콜라이더 찾기
    //    Collider2D[] platformColliders = Physics2D.OverlapCircleAll(transform.position, 10f, platformLayerMask);

    //    foreach (Collider2D platformCollider in platformColliders) //콜라이더 무시
    //    {
    //        if(platformCollider != null && platformCollider.CompareTag("Ground"))
    //        {
    //            Physics2D.IgnoreCollision(playerCollider, platformCollider, ignore);
    //        }
    //    }

    //    yield return null;
    //}

    //public IEnumerator ResetIgnoreCollision(float delay) //콜라이더 무시 상태 초기화
    //{
    //    yield return new WaitForSeconds(delay);


    //    StartCoroutine(IgnorePlatformCollision(false));
    //    isIgnoringCollision = false;
    //}

    public IEnumerator InvincibleEffect()
    {
        while (isInvincible)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f);
            yield return null;
        }
        
    }
        
}
