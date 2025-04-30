using System;
using UnityEngine;

public class HitBox : MonoBehaviour // 1회 공격 당의 캐싱이 필요할 수 있음
{
    private float damage;
    // hitBox 자체는 default Layer 어야 트리거 인식 자체는 하게 된다.
    public LayerMask? ownerLayer { get; private set; } = null;
    
    private Collider2D _collider;
    
    public bool isKnockBack = false;
    public float knockBackForce = 0f;

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
    
    public HitBox SetSize(float size)
    {
        transform.localScale = new Vector3(size, size, 1f);
        return this;
    }

    public HitBox SetSize(float x, float y)
    {
        transform.localScale = new Vector3(x, y, 1f);
        return this;
    }

    public HitBox SetLocalPos(Vector2 position)
    {
        transform.localPosition = position;
        return this;
    }
    
    public HitBox SetKnockBack(float knockBackForce)
    {
        isKnockBack = true;
        this.knockBackForce = knockBackForce;

        return this;
    }

    public HitBox Fire()
    {
        gameObject.SetActive(true);
        return this;
    }

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    // notice: 오브젝트가 enable 될 때 트리거 인식 안되는 현상 발생
    private void OnEnable()
    {
        // 임시 수정 - 근거리 공격 인식 안되는 현상 발생
        if(transform.parent.gameObject.layer != LayerMask.NameToLayer("Default"))
        {
            ownerLayer = transform.parent.gameObject.layer;
        }
        _collider.enabled = true;
    }

    private void OnDisable()
    {
        _collider.enabled = false;
        
        isKnockBack = false;
        knockBackForce = 0f;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // notice: UI에게 notify하는 방식으로 관리하기
        // Vector3 textPosition = transform.position + Vector3.up * 1f;
        // DamageTextManager.Instance.ShowDamage(textPosition, (int)damage);
        
        if (!other.TryGetComponent(out IDamagable damagable) || ownerLayer == other.gameObject.layer) return;

        
        if (isKnockBack && other.attachedRigidbody) { other.attachedRigidbody.AddForce((other.transform.position - transform.position).normalized * knockBackForce, ForceMode2D.Impulse); }
        // notice: 캐스팅 대상이 플레이어인 경우
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            damagable.GetDamage(damage);
            return;
        }

        // notice: getComponent 비용이 크니 방어처리를 GetDamage 쪽에서 해주는 게 좋을 듯
        ControllerPlayer player = other.gameObject.GetComponent<ControllerPlayer>();

        if (player && !player.isInvincible)
        {
            damagable.GetDamage(damage);
        }
    }
}
