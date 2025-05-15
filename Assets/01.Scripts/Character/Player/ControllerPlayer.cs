using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;

public class ControllerPlayer : MonoBehaviour
{
    public Vector2 inputVec;
    private float speed;

    [Header("DashParameter")]
    public float dashDistance; //점프 거리
    public float dashTime; //대쉬 시간

    [Header("JumpParameter")]
    public float jumpPower; //점프력
    public int jumplimit; //점프 횟수 제한
    public int currentJumpCount; //현재 점프 카운트
    public bool CanJump => jumplimit > currentJumpCount;
    public LayerMask platformLayerMask; 
    public LayerMask invincibilityLayerMask; 

    public bool isGround; 
    public bool isDashing = false; 
    public bool isAttacking = false; 
    public bool isInvincible = false; 
    private bool dashBuffered = false;
    public bool isAlive = true;
    public bool canAttack = true;
    public bool canSkill = true;
    public bool canDash = true;

    public Rigidbody2D rigid;
    public Animator animator;
    public Collider2D playerCollider;
    public SpriteRenderer spriteRenderer;

    public PlayerInteraction interaction;
    public CharacterStatus status;
    PlayerSound playerSound;

    // FSM
    private Dictionary<PlayerState, PlayerStateMachine> states = new ();
    public PlayerState currentState;
    public PlayerState previousState;

    public bool isFacingRight = true;

    [SerializeField] private float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public bool isOnLadder = false;

    private float lastDirectionChangeTime = 0f;
    private float directionChangeCooldown = 0.1f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        interaction = GetComponent<PlayerInteraction>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        status = GetComponent<CharacterStatus>();
        playerSound = GetComponent<PlayerSound>();

        InitStateMachine();
    }

    private void InitStateMachine()
    {
        
        states.Add(PlayerState.Idle, new IdleState(this));
        states.Add(PlayerState.Run, new RunState(this));
        states.Add(PlayerState.Jump, new JumpState(this));
        states.Add(PlayerState.Dash, new DashState(this));
        states.Add(PlayerState.Interaction, new InteractionState(this));
        states.Add(PlayerState.Climb, new ClimbState(this));
        states.Add(PlayerState.Slide, new SlideState(this));
        states.Add(PlayerState.Fall, new FallState(this));

        // �ʱ� ���� ����
        if (currentState == 0 || !states.ContainsKey(currentState))
        {
            ChangeState(PlayerState.Idle);
        }
    }

    void Start()
    {
        Transform firePoint = transform.Find("FirePoint");
        if (firePoint != null)
        {
            SkillController.Instance.skillSpawnPoint2 = firePoint;
        }
        else
        {
            Debug.LogWarning("FirePoint�� ã�� �� �����ϴ�!");
        }

        canAttack = true;
        canSkill = true;
    }

    public void ChangeState(PlayerState newState)
    {
        if (!isAlive) return;

        previousState = currentState;

       
        if (states.ContainsKey(currentState))
        {
            states[currentState].Exit();
        }

        
        currentState = newState;

        
        if (states.ContainsKey(currentState))
        {
            states[currentState].Enter();
        }
    }

    private void Update()
    {
        
        if (states.ContainsKey(currentState))
        {
            states[currentState].Update();
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (!isGround &&  rigid.velocity.y < 0)
        {
            if (!stateInfo.IsTag("Attack") && !stateInfo.IsTag("Ladder") && !stateInfo.IsTag("WallSlide") && !stateInfo.IsTag("Dash"))
            {
                if (!animator.GetBool("IsFall"))
                {
                    animator.SetTrigger("FallTrigger");
                    animator.SetBool("IsFall", true);
                }
            }
        }
        else
        {
            animator.SetBool("IsFall", false);
        }

        if (dashBuffered)
        {
            if (!stateInfo.IsTag("Turn"))
            {
                dashBuffered = false;

                if (states.ContainsKey(currentState))
                {
                    states[currentState].OnDash();
                }
            }
        }

        //CheckWall();
    }
    private void FixedUpdate()
    {
        
        if (states.ContainsKey(currentState))
        {
            states[currentState].FixedUpdate();
        }

        UpdateGroundCheck();
      
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) return;
        inputVec = value.Get<Vector2>();

        if (states.ContainsKey(currentState))
        {
            states[currentState].OnMove(inputVec);
        }
    }

   
    void OnJump(InputValue value)
    {
        if (!isAlive) return;
        if (value.isPressed)
        {
            var weaponData = SystemManager.Instance.weaponManager.GetCurrentWeaponData();
            var skillController = SkillController.Instance;

            if (weaponData != null && weaponData.Type == WeaponType.Sword && skillController.comboAttack.attackIndex >= 3)
            {
                Debug.Log("콤보가 3 이상이라 점프할 수 없습니다!");
                return;
            }

            if (states.ContainsKey(currentState))
            {
                states[currentState].OnJump();
            }
        }
    }

    void OnDash(InputValue value) //�뽬 Ű �Է�
    {
        if (!isAlive) return;
        if (!canDash) return;

        var skillController = SkillController.Instance;

        if (value.isPressed)
        {
            if (states.ContainsKey(currentState))
            {
                if (skillController.isSkillPlaying)
                {
                    SystemManager.Instance.actionBufferUtil.BufferAction(
                        "Dash",
                        () => !skillController.isSkillPlaying,
                        () => states[currentState].OnDash()
                    );
                }
                else
                {
                    states[currentState].OnDash();
                }
            }
        }
    }    

    void OnInteraction() //��ȣ �ۿ� Ű �Է�
    {
        if (!isAlive) return;
       
        if (states.ContainsKey(currentState))
        {
            states[currentState].OnInteraction();
        }
        Debug.Log("F: ��ȣ�ۿ�");
    }

    void OnInventory() //�κ��丮 Ű �Է�
    {
        Debug.Log("I: �κ��丮 ����");
    }

    void OnMenu() //�޴� Ű �Է�
    {
        Debug.Log("Esc: �޴�â ����");
    }

    void OnOtherWeapon() //���⺯ȯ Ű �Է�
    {
        SystemManager.Instance.weaponManager.SwapWeapon();
        Debug.Log("Z: �ٸ������ ��ȯ");
    }

    
    public void UpdateDirection() //���� ��ȯ
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        bool isAttack = stateInfo.IsTag("Attack");
        bool isMoving = Mathf.Abs(inputVec.x) > 0.01f;

        if (inputVec.x < 0 && isFacingRight && !isAttack)
        {
            // ������ �� �������� �ٲ�
            isFacingRight = false;
            transform.localEulerAngles = new Vector3(0, 180, 0);
            lastDirectionChangeTime = Time.time;

            canSkill = false; 
            canAttack = false;
            StartCoroutine(EnableSkillAfterDelay());
        }
        else if (inputVec.x > 0 && !isFacingRight && !isAttack)
        {
            // ���� �� ���������� �ٲ�
            isFacingRight = true;
            transform.localEulerAngles = new Vector3(0, 0, 0);
            lastDirectionChangeTime = Time.time;

            canSkill = false;
            canAttack = false;
            StartCoroutine(EnableSkillAfterDelay());
        }
    }

    private IEnumerator EnableSkillAfterDelay()
    {
        yield return new WaitForSeconds(directionChangeCooldown);
        canSkill = true;
        canAttack = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (states.ContainsKey(currentState))
        {
            states[currentState].OnCollisionEnter(collision);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            playerSound.LandSound();
            isGround = true;
            animator.SetBool("IsJump", false);
            currentJumpCount = 0;
            rigid.velocity = Vector3.zero;
        }
       
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

        if (collision.gameObject.layer == LayerMask.NameToLayer("Climb") && inputVec.y < 0)
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

        // 무적 시 
        if (isInvincible)
        {
            StartCoroutine(InvincibleEffect());

            Collider2D[] Colliders = Physics2D.OverlapCircleAll(transform.position, 10f, invincibilityLayerMask);

            foreach (Collider2D Collider in Colliders) //�ݶ��̴� ����
            {
                if (Collider != null )
                {
                    Physics2D.IgnoreCollision(playerCollider, Collider, true);
                }
            }
        }
        else
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            ResetIgnoredCollision(); //무적 판정 종료
        }   
    }

    public void ResetIgnoredCollision()
    {
        if (isGround)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f, invincibilityLayerMask);

            foreach (Collider2D collider in colliders)
            {
                Physics2D.IgnoreCollision(playerCollider, collider, false);
            }
        }
    }

    public IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        animator.SetBool("IsRun", false);
        rigid.velocity = new Vector2(0, rigid.velocity.y);

        yield return new WaitForSeconds(0.35f);

        BoltsPool.Instance.CreateMelee(transform, 10);

        yield return new WaitForSeconds(0.1f);
              
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
        BoltsPool.Instance.DestroyMelee(transform);

        
        if (inputVec.x != 0)
        {
            animator.SetBool("IsRun", true);
            rigid.velocity = new Vector2(inputVec.x * speed, rigid.velocity.y);
        }
    }
 
    public IEnumerator InvincibleEffect()
    {
        while (isInvincible)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f);
            yield return null;
        }  
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }

    public void UpdateGroundCheck()
    {
        if (currentState != PlayerState.Climb)
        {
            isGround = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);
        }
    }

    public void OnAttackAnimationEnd()
    {
        if (inputVec.x == 0)
            ChangeState(PlayerState.Idle);
        else
            ChangeState(PlayerState.Run);
    }
}
