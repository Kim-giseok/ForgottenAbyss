using System;
using UnityEngine;

public abstract class Projectile: MonoBehaviour
{
    protected float currTime;  
    public float duration;
    public float speed;

    protected Rigidbody2D rigidbody;
    protected Collider2D collider;
    protected SpriteRenderer spriteRenderer;
    
    // public void SetSprite(Sprite sprite) => spriteRenderer.sprite = sprite;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        // 그라운드, 플레이어, 에너미 와의 충돌이 아닌 경우 무시 필요
        if (gameObject.layer == LayerMask.NameToLayer("Default")) return;
        
        if (gameObject.layer == other.gameObject.layer) return;
        ProjectileManager.Instance.DestroyProjectile(gameObject);
    }
}