using System;
using UnityEngine;

public class HitBox : MonoBehaviour // 1회 공격 당의 캐싱이 필요할 수 있음
{
    private float damage;
    private LayerMask ownerLayer;
    
    private Collider2D collider;

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    public void SetOwner(Transform owner)
    {
        this.ownerLayer = owner.gameObject.layer;
    }

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    // notice: 오브젝트가 enable 될 때 트리거 인식 안되는 현상 발생
    private void OnEnable()
    {
        // 임시 수정 - 근거리 공격 인식 안되는 현상 발생
        if (transform.parent.gameObject.layer != LayerMask.NameToLayer("Default"))
        {
            gameObject.layer = transform.parent.gameObject.layer;
        }
        collider.enabled = true;
    }

    private void OnDisable()
    {
        collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out IDamagable damagable) || ownerLayer == other.gameObject.layer) return;
        damagable.GetDamage(damage);
    }
}
