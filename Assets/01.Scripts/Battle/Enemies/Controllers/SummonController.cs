using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SummonController: EnemyBaseController
{
    public Transform target {get; private set;}
    private bool isTargetAttached = true;
    
    public Rigidbody2D tRigidbody {get; private set;}
    private Collider2D tCollider;
    private SpriteRenderer tRenderer;
    
    protected override void Awake()
    {
        base.Awake();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        
        tRigidbody  = target.GetComponent<Rigidbody2D>();
        tRenderer = target.GetComponent<SpriteRenderer>();
    }

    public void Set(SummonSkillManager.Skill skill)
    {
        var (enemy, node) = SummonSkillManager.Get(skill);
        animationHandler.SetController(EnemiesAnimator.animators[enemy.ToString()]);

        // notice: 머신도 제거되는 지 체크 후 이벤트 제거 필요
        machine.OnLooped += () => { Destroy(gameObject); };
        machine.Define(node);
    }

    private void OnEnable()
    {
        if (!isTargetAttached) return;
        // 플레이어 비/활성화가 잠시 필요 - agent 쪽에서 인식 처리만 잘되면 됨
        tRenderer.enabled = false;
    }

    public void Update()
    {
        if (!isTargetAttached) return;
        target.transform.position = transform.position;
    }

    private void OnDestroy()
    {
        if (!isTargetAttached) return;
        tRigidbody.velocity = Vector2.zero;
        tRenderer.enabled = true;
    }
}