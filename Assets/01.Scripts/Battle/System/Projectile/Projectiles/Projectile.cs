using System;
using UnityEngine;

public class Projectile: MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private Transform target;

    private float currTime;
    public float duration;
    public float speed;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        currTime = 0;
    }

    public void FixedUpdate()
    {
        currTime += Time.fixedDeltaTime;
        if (currTime >= duration)
        {
            ProjectileManager.Instance.DestroyProjectile(gameObject);
            return;
        }
        

        rigidbody.velocity = transform.up * speed;
    }
}