using System.Collections.Generic;
using UnityEngine;

public class Bolt: MonoBehaviour
{
    public List<GameObject> targets; // 현재 발사체에 등록이 된 목록
    public bool isStarted { get; private set; } = false;
    
    public HitBox hitBox { get; private set; }
    // 공통변수
    public float currTime { get; private set; } = 0f;
    public float currNodeTime { get; private set; } = 0f;
    public Vector2 direction { get; private set; } // 발사체의 방향은 공통 변수로 관리
    
    // Node가 자체적으로 가진다. - 총 합에 해당하는 duration
    public float duration { get; private set; } = 1;
    public float speed { get; private set; } = 10;

    // transform 에서 사이즈도 처리
    public Rigidbody2D rigidbody { get; private set; }
    public Collider2D collider { get; private set; }
    public SpriteRenderer renderer { get; private set; }
    // 애니메이터는 한개지만 내부 애니메이션 실행을 목적으로 이용
    public BoltAnimHandler animHandler { get; private set; }
    
    public StepMachine machine { get; private set; } // 등록 자체를 순차 등록
    protected List<BoltEffect> effects = new();

    public void SetSprite(Sprite sprite) => renderer.sprite = sprite;
    public Bolt SetSize(float size)
    {
        transform.localScale = new Vector3(size, size, 1);
        return this;
    }

    public Bolt SetDamage(float damage)
    {
        hitBox.SetDamage(damage);
        return this;
    }
    
    public Bolt SetDuration(float duration)
    {
        this.duration = duration;
        return this;
    }

    public Bolt SetEffect(BoltEffect effect)
    {
        effects.Add(effect);
        return this;
    }

    public void AddEffect(BoltEffect effect) => effects.Add(effect);
    private void Play() => this.isStarted = true;

    public Bolt SetSpeed(float speed)
    {
        this.speed = speed;
        return this;
    }

    public Bolt SetDirection(Vector2 direction)
    {
        this.direction = direction;
        this.direction.Normalize();
        return this;
    }

    public Bolt SetDegree(float degree)
    {
        float radian = degree * Mathf.Deg2Rad;
        transform.rotation = Quaternion.Euler(0, 0, degree);
        direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        return this;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public Bolt Fire()
    {
        gameObject.SetActive(true);
        machine.Start();
        Play();
        return this;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        renderer = GetComponent<SpriteRenderer>();
        
        animHandler = GetComponent<BoltAnimHandler>();
        hitBox = GetComponent<HitBox>();
        machine = new(this);
    }
    protected void Update()
    {
        if (!isStarted) return;
        currTime += Time.deltaTime;
        machine.Run();
    }

    protected virtual void OnEnable()
    {
        currTime = 0;
    }

    // 삭제가 없으므로
    private void OnDisable()
    {
        machine.Clear();
        effects.Clear();
        rigidbody.velocity = Vector2.zero;
        isStarted = false;
    }

    protected virtual void FixedUpdate()
    {
        if (currTime >= duration) { BoltsPool.Instance.Disable(gameObject); }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // 그라운드, 플레이어, 에너미 와의 충돌이 아닌 경우 무시 필요(임시 해결) // 총알끼리 부딪힘
        if (other.gameObject.layer == LayerMask.NameToLayer("Default") || other.gameObject.layer == gameObject.layer) return;
        
        // 레이어 자체는 모두 감지가 필요하므로 충돌 비교 레이어를 필드로 따로 둠
        if (hitBox.ownerLayer == other.gameObject.layer) return;
       
        foreach (BoltEffect effect in effects)
        {
            effect.Execute(other);
        }
        
        Debug.Log(other.gameObject.name);
        
        BoltsPool.Instance.Disable(gameObject);
    }
}