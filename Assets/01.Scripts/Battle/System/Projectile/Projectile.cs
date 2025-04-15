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
        if (gameObject.layer == other.gameObject.layer) return;
        ProjectileManager.Instance.DestroyProjectile(gameObject);
    }
}