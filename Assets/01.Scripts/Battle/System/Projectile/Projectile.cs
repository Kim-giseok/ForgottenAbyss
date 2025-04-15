using System;
using UnityEngine;

public abstract class Projectile: MonoBehaviour
{
    protected float currTime;
    protected float duration;
    
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

    protected void OnEnable()
    {
        currTime = 0;
    }

    protected virtual void FixedUpdate()
    {
        currTime += Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ProjectileManager.Instance.DestroyProjectile(gameObject);
    }
}