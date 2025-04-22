using System;
using System.Collections.Generic;
using UnityEngine;

public class Bolt: MonoBehaviour
{
    public HitBox hitBox;
    
    // 공통변수
    public float currTime;
    public Vector2 currDirection; // 발사체의 방향은 공통 변수로 관리
    public float duration; // Node가 자체적으로 가진다. - 총 합에 해당하는 duration
    public float speed;

    // transform 에서 사이즈도 처리
    public Rigidbody2D rigidbody { get; private set; }
    public Collider2D collider { get; private set; }
    public SpriteRenderer renderer;
    public Animator animator; // 애니메이터는 한개지만 내부 애니메이션 실행을 목적으로 이용
    public BoltAnimHandler animHandler;
    
    public StepMachine machine { get; private set; } = new(); // 등록 자체를 순차 등록
    protected List<BoltEffect> effects = new();
    
    public void SetSprite(Sprite sprite) => renderer.sprite = sprite;
    public void SetSize(float size) => transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
    public void AddEffect(BoltEffect effect) => effects.Add(effect);

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        renderer = GetComponent<SpriteRenderer>();
        
        hitBox = GetComponent<HitBox>();
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
        // rigidbody.velocity = transform.right * speed;

        if (currTime >= duration)
        {
            BoltManager.Instance.Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // 그라운드, 플레이어, 에너미 와의 충돌이 아닌 경우 무시 필요(임시 해결) // 총알끼리 부딪힘
        if (other.gameObject.layer == LayerMask.NameToLayer("Default") || other.gameObject.layer == gameObject.layer) return;
        
        // 레이어 자체는 모두 감지가 필요하므로 충돌 비교 레이어를 필드로 따로 둠
        if (hitBox.ownerLayer == other.gameObject.layer) return;
       
        BoltManager.Instance.Destroy(gameObject);

        foreach (BoltEffect effect in effects)
        {
            effect.Execute(other);
        }
    }
}