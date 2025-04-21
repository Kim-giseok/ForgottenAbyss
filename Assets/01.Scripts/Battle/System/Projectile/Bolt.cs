using System;
using UnityEngine;

public abstract class Bolt: MonoBehaviour
{
    protected float currTime;  
    
    public float duration; // Node가 자체적으로 가진다.
    public float speed;

    // transform 에서 사이즈도 처리
    protected Rigidbody2D rigidbody;
    protected Collider2D collider;
    
    protected SpriteRenderer renderer;
    protected Animator animator; // 애니메이터는 한개지만 내부 애니메이션 실행을 목적으로 이용
    protected BoltAnimHandler animHandler;
    
    protected BoltStepMachine machine = new(); // 등록 자체를 순차 등록
    
    // public void SetSprite(Sprite sprite) => spriteRenderer.sprite = sprite;
    public void SetSize()
    {
        // transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1) * 3f;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    protected void Update()
    {
        machine.Run();
    }

    protected virtual void OnEnable()
    {
        currTime = 0;
    }

    protected virtual void FixedUpdate()
    {
        currTime += Time.fixedDeltaTime;
        rigidbody.velocity = transform.right * speed;

        if (currTime >= duration)
        {
            ProjectileManager.Instance.DestroyProjectile(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // 그라운드, 플레이어, 에너미 와의 충돌이 아닌 경우 무시 필요(임시 해결)
        if (other.gameObject.layer == LayerMask.NameToLayer("Default")) return;

        if (gameObject.layer == other.gameObject.layer) return;
        ProjectileManager.Instance.DestroyProjectile(gameObject);
    }
}