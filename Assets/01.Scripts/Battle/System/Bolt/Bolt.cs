using UnityEngine;

public class Bolt: MonoBehaviour
{
    private BoltBuilder attr;
    private HitBox hitBox;
    private Collider2D collider;

    private void Awake()
    {
        attr = GetComponent<BoltBuilder>();
        hitBox = GetComponent<HitBox>();
        collider = GetComponent<Collider2D>();
        
    }

    protected void Update()
    {
        if (!attr.isStarted) return;
        attr.currTime += Time.deltaTime;
        attr.machine.Run();
    }

    private void OnEnable()
    {
        // awake가 한번 느리면 인식이 안되는 현상 발생
        attr.trailRenderer.Clear();
        attr.currTime = 0;
        attr.hitBox.gameObject.SetActive(true);
        attr.animHandler.Play("None");
    }

    // 삭제가 없으므로
    private void OnDisable()
    {
        // fix: 시간이 완료된 경우 마지막 노드로 클리어 필요
        attr.Renderer.sprite = BoltsPool.Instance.GetSprite("circle");
        attr.Renderer.color = Color.magenta;
        transform.localScale = new Vector3(0.2f, 0.2f, 1);
        
        // boltBuilder 에게 한번 더 요청하는 방식으로 간결하게 해소하기
        attr.machine.Clear();
        attr.effects.Clear();
        attr.Rigidbody.velocity = Vector2.zero;

        attr.trailRenderer.enabled = true;
        attr.trailRenderer.time = 0.2f;
        attr.SetParticle(false);
        
        hitBox.enabled = true;
        collider.enabled = true;
        
        attr.isStarted = false;
        
    }

    protected virtual void FixedUpdate()
    {
        if (attr.currTime >= attr.duration)
        {
            BoltsPool.Instance.Disable(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // 그라운드, 플레이어, 에너미 와의 충돌이 아닌 경우 무시 필요(임시 해결) // 총알끼리 부딪힘
        if(other.gameObject.layer != LayerMask.GetMask("Enemy") && other.gameObject.layer != LayerMask.GetMask("Player")) return;
        if (other.gameObject.layer == gameObject.layer) return;
        // 레이어 자체는 모두 감지가 필요하므로 충돌 비교 레이어를 필드로 따로 둠
        if (attr.hitBox.ownerLayer == other.gameObject.layer) return;
       
        foreach (BoltEffect effect in attr.effects)
        {
            effect.Connect(attr);
            effect.Execute(other);
        }
        
        Debug.LogWarning(other.name);

        if (attr.effects.Count == 0) { BoltsPool.Instance.Disable(gameObject); }
    }
}