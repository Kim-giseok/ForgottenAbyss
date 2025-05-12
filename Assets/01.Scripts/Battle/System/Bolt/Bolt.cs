using System;
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
    public Vector3 direction { get; private set; } // 발사체의 방향은 공통 변수로 관리
    
    // Node가 자체적으로 가진다. - 총 합에 해당하는 duration
    public float duration { get; private set; } = 1;
    public float speed { get; private set; } = 10;

    // transform 에서 사이즈도 처리
    public Rigidbody2D Rigidbody { get; private set; }
    public Collider2D Collider { get; private set; }
    public SpriteRenderer Renderer { get; private set; }
    // 애니메이터는 한개지만 내부 애니메이션 실행을 목적으로 이용
    public BoltAnimHandler animHandler { get; private set; }
    public TrailRenderer trailRenderer { get; private set; }
    public LineRenderer lineRenderer { get; private set; }
    
    public StepMachine machine { get; private set; } // 등록 자체를 순차 등록
    protected List<BoltEffect> effects = new();

    public void SetSprite(Sprite sprite) => Renderer.sprite = sprite;
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

    public Bolt SetEffect(Bolts.EffectType effectType)
    {
        effects.Add(Bolts.GetEffect(effectType));
        return this;
    }

    public Bolt SetSprite(string name)
    { 
        Renderer.sprite = BoltsPool.Instance.GetSprite(name);
        return this;
    }
    
    
    public Bolt SetPosition(Vector3 position)
    {
        transform.position = position;
        return this;
    }

    public Bolt SetVFX()
    {
        hitBox.gameObject.SetActive(false);
        hitBox.SetDamage(0);
        effects.Add(Bolts.GetEffect(Bolts.EffectType.Penetration));
        return this;
    }

    public void AddEffect(BoltEffect effect) => effects.Add(effect);
    public void Play() => this.isStarted = true;

    public Bolt SetSpeed(float speed)
    {
        this.speed = speed;
        return this;
    }

    public Bolt SetColor(Color color)
    {
        this.Renderer.color = color;
        return this;
    }

    public Bolt SetDirection(Vector3 direction)
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
        direction.Normalize();
        return this;
    }

    public Bolt SetRenderer(Action<SpriteRenderer> callback)
    {
        callback?.Invoke(Renderer);
        return this;
    }
    
    public Bolt SetTrail(bool enabled)
    {
        trailRenderer.enabled = enabled;
        return this;
    }

    public Bolt SetTrailCurve(BoltsPool.TrailType trailType)
    {
        // Find 방식 비용 문제 발생
        // var selectedCurve = BoltsPool.Instance.trailAnimCurves[trailType];
        // trailRenderer.widthCurve = selectedCurve.curve;

        if (trailType == BoltsPool.TrailType.Laser) { trailRenderer.time = 1f; }
        return this;
    }
    
    public Bolt SetKnockBack(float amount)
    {
        hitBox.SetKnockBack(amount);
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
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
        Renderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        
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

    private void OnEnable()
    {
        trailRenderer.Clear();
        currTime = 0;
        hitBox.gameObject.SetActive(true);
        animHandler.Play("None");
    }

    // 삭제가 없으므로
    private void OnDisable()
    {
        // fix: 시간이 완료된 경우 마지막 노드로 클리어 필요
        Renderer.sprite = BoltsPool.Instance.GetSprite("circle");
        Renderer.color = Color.magenta;
        transform.localScale = new Vector3(0.2f, 0.2f, 1);
        
        
        machine.Clear();
        effects.Clear();
        Rigidbody.velocity = Vector2.zero;

        trailRenderer.enabled = true;
        trailRenderer.time = 0.2f;
        
        isStarted = false;
        
    }

    protected virtual void FixedUpdate()
    {
        if (currTime >= duration)
        {
            BoltsPool.Instance.Disable(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // 그라운드, 플레이어, 에너미 와의 충돌이 아닌 경우 무시 필요(임시 해결) // 총알끼리 부딪힘
        if (other.gameObject.layer == LayerMask.NameToLayer("Default") || other.gameObject.layer == gameObject.layer) return;
        // 레이어 자체는 모두 감지가 필요하므로 충돌 비교 레이어를 필드로 따로 둠
        if (hitBox.ownerLayer == other.gameObject.layer) return;
       
        foreach (BoltEffect effect in effects)
        {
            effect.Connect(this);
            effect.Execute(other);
        }

        if (effects.Count == 0) { BoltsPool.Instance.Disable(gameObject); }
    }
}