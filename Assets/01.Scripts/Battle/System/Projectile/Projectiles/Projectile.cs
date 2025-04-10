using System.Collections.Generic;
using UnityEngine;

public class Projectile: MonoBehaviour
{
    public List<ProjectileAttr> attributes = new();
    
    public Rigidbody2D rigidbody { get; private set; }
    public Collider2D collider { get; private set; }
    public Transform target { get; private set; }

    public float currTime { get; private set; }
    public float duration;
    public float speed;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform; // 내가 공격하는 경우인 경우 대상이 달라질 수 있음
    }

    private void OnEnable()
    {
        currTime = 0;
        attributes.ForEach(attribute => attribute.Start());
    }

    private void Start()
    {
        attributes.ForEach(attribute => attribute.Start());
    }
    
    public void AddAttribute(params ProjectileAttr[] newAttributes)
    {
        attributes.Clear();
        foreach (var attribute in newAttributes)
        {
            attribute.Connect(this);
            attributes.Add(attribute);
        }
    }

    public void FixedUpdate()
    {
        currTime += Time.fixedDeltaTime;
        if (currTime >= duration) { ProjectileManager.Instance.DestroyProjectile(gameObject); return; }
        
        // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
        attributes.ForEach(attribute => attribute.Update());
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        attributes.ForEach(attribute => attribute.TriggerEnter(other));
    }
}