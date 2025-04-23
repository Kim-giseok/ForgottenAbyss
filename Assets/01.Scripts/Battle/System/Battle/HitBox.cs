using System;
using UnityEngine;

public class HitBox : MonoBehaviour // 1회 공격 당의 캐싱이 필요할 수 있음
{
    private float damage;
    // hitBox 자체는 default Layer 어야 트리거 인식 자체는 하게 된다.
    public LayerMask? ownerLayer { get; private set; } = null;
    
    private Collider2D collider;

    public HitBox SetDamage(float damage)
    {
        this.damage = damage;
        return this;
    }
    public HitBox SetOwner(Transform owner)
    {
        this.ownerLayer = owner.gameObject.layer;
        return this;
    }

    public HitBox SetLocalPos(Vector2 position)
    {
        this.transform.localPosition = position;
        return this;
    }

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    // notice: 오브젝트가 enable 될 때 트리거 인식 안되는 현상 발생
    private void OnEnable()
    {
        // 임시 수정 - 근거리 공격 인식 안되는 현상 발생
        if(transform.parent.gameObject.layer != LayerMask.NameToLayer("Default"))
        {
            ownerLayer = transform.parent.gameObject.layer;
        }
        collider.enabled = true;
    }

    private void OnDisable()
    {
        collider.enabled = false;
    }

    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // notice: UI에게 notify하는 방식으로 관리하기
        // Vector3 textPosition = transform.position + Vector3.up * 1f;
        // DamageTextManager.Instance.ShowDamage(textPosition, (int)damage);
        
        if (!other.TryGetComponent(out IDamagable damagable) || ownerLayer == other.gameObject.layer) return;

        // notice: 캐스팅 대상이 플레이어인 경우
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            damagable.GetDamage(damage);
            return;
        }

        ControllerPlayer player = other.gameObject.GetComponent<ControllerPlayer>();

        if (player != null && !player.isInvincible) { damagable.GetDamage(damage); }
    }
}
