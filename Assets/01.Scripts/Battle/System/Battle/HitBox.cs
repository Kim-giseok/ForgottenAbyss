using System;
using UnityEngine;

public class HitBox : MonoBehaviour // 1회 공격 당의 캐싱이 필요할 수 있음
{
    private float damage;
    // hitBox 자체는 default Layer 어야 트리거 인식 자체는 하게 된다.

    public Transform owner;
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
        this.owner = owner;
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

    // 이펙트 연결이 될 수 있음
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

        // notice: 플레이어 조작시 rigid가 갱신되면서 넉백이 캔슬됨
        if (isKnockBack && other != null && other.attachedRigidbody != null)
        {
            if (other.transform == null || owner == null || owner.Equals(null)) return;

            Vector2 direction = ((Vector2)other.transform.position - (Vector2)owner.position).normalized;
            other.attachedRigidbody.AddForce(direction * knockBackForce, ForceMode2D.Impulse);
        }
        
        // notice: 캐스팅 대상이 플레이어인 경우
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            damagable.GetDamage(damage);

            // 임시로 데미지 텍스트, 카메라 쉐이크 넣어둠
            var enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                DamageTextManager.Instance.ShowDamage(other.transform.position, (int)damage, false);
                // GameManager.Instance.cameraShake.Shake(); 
            }
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
