using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerPlayer : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed; //이동속도
    public float jumpPower; //점프력
    public float dashDistance; //대쉬거리
    public float dashTime; //대쉬지속시간

    private bool isGround; //땅 밟고 있는지 여부
    private bool isDashing = false; //대쉬 여부

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            rigid.velocity = new Vector2(inputVec.x * speed, rigid.velocity.y);
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && isGround) //땅에 닿아 있을 때 점프 가능
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isGround = false;
        }
    }

    void OnDash(InputValue value)
    {
        if (value.isPressed && !isDashing && isGround)
        {
            StartCoroutine(Dash());
        }
    }

    void OnAttack()
    {
        Debug.Log("일반공격");
    }

    void OnFirstSkill()
    {
        Debug.Log("스킬1");
    }

    void OnSecondSkill()
    {
        Debug.Log("스킬2");
    }

    void OnSpecialSkill()
    {
        Debug.Log("특수 스킬");
    }

    void OnInteraction()
    {
        Debug.Log("상호작용 시작");
    }

    void OnInventory()
    {
        Debug.Log("인벤토리 열기");
    }

    void OnMenu()
    {
        Debug.Log("메뉴창 열기");
    }

    void OnOtherWeapon()
    {
        Debug.Log("다른무기로 변환");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
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
}
