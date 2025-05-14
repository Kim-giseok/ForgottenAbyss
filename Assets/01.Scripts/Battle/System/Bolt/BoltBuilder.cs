using System;
using System.Collections.Generic;
using UnityEngine;

public class BoltBuilder: MonoBehaviour
{
    public List<GameObject> targets { get; private set; }// 현재 발사체에 등록이 된 목록
    [HideInInspector] public bool isStarted;
    
    public HitBox hitBox { get; private set; }
    // 공통변수
    [HideInInspector] public float currTime;
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
    public List<BoltEffect> effects = new();
    public bool IsParticle { get; private set; }

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
    
    public void SetSprite(Sprite sprite) => Renderer.sprite = sprite;
    public BoltBuilder SetSize(float size)
    {
        transform.localScale = new Vector3(size, size, 1);
        return this;
    }

    public BoltBuilder SetDamage(float damage)
    {
        hitBox.SetDamage(damage);
        return this;
    }
    
    public BoltBuilder SetDuration(float duration)
    {
        this.duration = duration;
        return this;
    }

    public BoltBuilder SetEffect(Bolts.EffectType effectType)
    {
        effects.Add(Bolts.GetEffect(effectType));
        return this;
    }

    public BoltBuilder SetSprite(string name)
    { 
        Renderer.sprite = BoltsPool.Instance.GetSprite(name);
        return this;
    }
    
    
    public BoltBuilder SetPosition(Vector3 position)
    {
        transform.position = position;
        return this;
    }

    public BoltBuilder SetVFX()
    {
        hitBox.gameObject.SetActive(false);
        hitBox.SetDamage(0);
        effects.Add(Bolts.GetEffect(Bolts.EffectType.Penetration));
        return this;
    }

    public void AddEffect(BoltEffect effect) => effects.Add(effect);
    public void Play() => this.isStarted = true;

    public BoltBuilder SetSpeed(float speed)
    {
        this.speed = speed;
        return this;
    }

    public BoltBuilder SetColor(Color color)
    {
        this.Renderer.color = color;
        return this;
    }

    public BoltBuilder SetDirection(Vector3 direction)
    {
        this.direction = direction;
        this.direction.Normalize();
        return this;
    }

    public BoltBuilder SetDegree(float degree)
    {
        float radian = degree * Mathf.Deg2Rad;
        transform.rotation = Quaternion.Euler(0, 0, degree);
        direction = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        direction.Normalize();
        return this;
    }

    public BoltBuilder SetRenderer(Action<SpriteRenderer> callback)
    {
        callback?.Invoke(Renderer);
        return this;
    }
    
    public BoltBuilder SetTrail(bool enabled)
    {
        trailRenderer.enabled = enabled;
        return this;
    }

    public BoltBuilder SetTrailCurve(BoltsPool.TrailType trailType)
    {
        // Find 방식 비용 문제 발생
        // var selectedCurve = BoltsPool.Instance.trailAnimCurves[trailType];
        // trailRenderer.widthCurve = selectedCurve.curve;

        if (trailType == BoltsPool.TrailType.Laser) { trailRenderer.time = 1f; }
        return this;
    }
    
    public BoltBuilder SetKnockBack(float amount)
    {
        hitBox.SetKnockBack(amount);
        return this;
    }

    public BoltBuilder Fire()
    {
        gameObject.SetActive(true);
        machine.Start();
        Play();
        return this;
    }

    public BoltBuilder SetParticle(bool isParticle)
    {
        IsParticle = isParticle;
        return this;
    }
}