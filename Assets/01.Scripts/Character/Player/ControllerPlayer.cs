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
    public float speed; //�̵��ӵ�
    public float jumpPower; //������
    public float dashDistance; //�뽬�Ÿ�
    public float dashTime; //�뽬���ӽð�
    public int jumplimit; //���� ���� Ƚ��
    public int currentJumpCount; //���� ���� Ƚ��
    public LayerMask platformLayerMask; //���� �� ������ �÷��� ���̾�
    public LayerMask invincibilityLayerMask; //���� ���¿��� ������ ���̾�

    //public LayerMask wallLayer; //�� ���� ���̾�
    //public float wallDistance; //�� ���� �Ÿ�

    //public bool isWallDetected; //�� ���� ����
    //public bool isWallClimbing; //��� ������ ������ ����
    //public RaycastHit2D wallHit;

    public bool isGround; //�� ��� �ִ��� ����
    public bool isDashing = false; //�뽬 ����
    public bool isAttacking = false; //���� ����
    //public bool isIgnoringCollision = false; //�ݶ��̴� �浹 ���� ����
    public bool isInvincible = false; //���� ���� ����
    private bool dashBuffered = false;
    public bool isAlive = true;

    public Rigidbody2D rigid;
    public Animator animator;
    public Collider2D playerCollider;
    public SpriteRenderer spriteRenderer;

    public PlayerInteraction interaction;
    public CharacterStatus status;

    // FSM ���� ����
    private Dictionary<PlayerState, PlayerStateMachine> states = new Dictionary<PlayerState, PlayerStateMachine>();
    public PlayerState currentState;
    public PlayerState previousState;

    public bool isFacingRight = true;

    [SerializeField] private float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public bool isOnLadder = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        interaction = GetComponent<PlayerInteraction>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        status = GetComponent<CharacterStatus>();

        // ���� �ӽ� �ʱ�ȭ
        InitStateMachine();
    }

    private void InitStateMachine()
    {
        // ���� ���
        states.Add(PlayerState.Idle, new IdleState(this));
        states.Add(PlayerState.Run, new RunState(this));
        states.Add(PlayerState.Jump, new JumpState(this));
        states.Add(PlayerState.Dash, new DashState(this));
        //states.Add(PlayerState.Attack, new AttackState(this));
        states.Add(PlayerState.Interaction, new InteractionState(this));
        states.Add(PlayerState.Climb, new ClimbState(this));
        states.Add(PlayerState.Slide, new SlideState(this));

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
    }

    public void ChangeState(PlayerState newState)
    {
        if (!isAlive) return;

        previousState = currentState;

        // ���� ���°� �ִٸ� Exit ȣ��
        if (states.ContainsKey(currentState))
        {
            states[currentState].Exit();
        }

        // ���� ����
        currentState = newState;

        // �� ������ Enter ȣ��
        if (states.ContainsKey(currentState))
        {
            states[currentState].Enter();
        }
    }

    private void Update()
    {
        // ���� ���� ������Ʈ
        if (states.ContainsKey(currentState))
        {
            states[currentState].Update();
            //Debug.Log($"{states[currentState]}");
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (!isGround &&  rigid.velocity.y < 0)
        {
            if (!stateInfo.IsTag("Attack") && !stateInfo.IsTag("Ladder") && !stateInfo.IsTag("WallSlide"))
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
        // ���� ���� FixedUpdate
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
            // ���� ���¿� ���� �Է� ����
            if (states.ContainsKey(currentState))
            {
                states[currentState].OnJump();
            }
        }
    }

    void OnDash(InputValue value) //�뽬 Ű �Է�
    {
        if (!isAlive) return;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool isTurn = stateInfo.IsTag("Turn");

        if (value.isPressed)
        {
            if (isTurn)
            {
                dashBuffered = true;
            }
            else
            {
                // �� �ִϸ��̼��� �ƴϸ� �ٷ� ��� ����
                if (states.ContainsKey(currentState))
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
        WeaponManager.Instance.SwapWeapon();
        Debug.Log("Z: �ٸ������ ��ȯ");
    }

    public void UpdateDirection() //���� ��ȯ
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        bool isLocomotion = stateInfo.IsTag("Locomotion");
        bool isAttack = stateInfo.IsTag("Attack");
        bool isMoving = Mathf.Abs(inputVec.x) > 0.01f;

        if (inputVec.x < 0 && isFacingRight && !isAttack)
        {
            // ������ �� �������� �ٲ�
            isFacingRight = false;
            transform.localEulerAngles = new Vector3(0, 180, 0);

            if (isLocomotion && isMoving)
                animator.SetTrigger("TurnTrigger");
        }
        else if (inputVec.x > 0 && !isFacingRight && !isAttack)
        {
            // ���� �� ���������� �ٲ�
            isFacingRight = true;
            transform.localEulerAngles = new Vector3(0, 0, 0);

            if (isLocomotion && isMoving)
                animator.SetTrigger("TurnTrigger");
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
       
        // ���� ���¿� �浹 �̺�Ʈ ����
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
        // ���� ���¿� �浹 ���� �̺�Ʈ ����
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

        // ���� ���� ���� ����
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
            // ���� ���� �� ���� ���·� ����
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            ResetIgnoredCollision(); //���� ���� ����
        }

        //if (!isInvincible)
        //{
        //    ResetIgnoredCollision(); //���� ���� ����
        //}

    }

    public void ResetIgnoredCollision() //���� ���� ����
    {
        // ���� �߿� �����ߴ� ��� �ݶ��̴����� �浹 ���� �ʱ�ȭ
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f, invincibilityLayerMask);

        foreach (Collider2D collider in colliders)
        {
            Physics2D.IgnoreCollision(playerCollider, collider, false);
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

        // ���� ���� �� ����Ű�� ������ �����ִٸ� �ӵ� ����
        if (inputVec.x != 0)
        {
            animator.SetBool("IsRun", true);
            rigid.velocity = new Vector2(inputVec.x * speed, rigid.velocity.y);
        }
    }

    public void IgnorePlatformCollision()
    {
        Collider2D[] platformColliders = Physics2D.OverlapCircleAll(transform.position, 10f, platformLayerMask);

        foreach (Collider2D platformCollider in platformColliders) //�ݶ��̴� ����
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
