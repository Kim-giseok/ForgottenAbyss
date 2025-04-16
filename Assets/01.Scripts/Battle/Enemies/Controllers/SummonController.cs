using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

// 중간에 풀어도 될 듯
public class SummonController: EnemyBaseController
{
    public int hitInformation; // 현재 공격에 대한 정보를 저장받는다.
    
    public Transform caster {get; private set;}
    private bool isCasterAttached = true;

    public Rigidbody2D cRigidbody {get; private set;}
    private Collider2D cCollider;
    private SpriteRenderer cRenderer;
    
    public Vector2 playerDirection {get; private set;}
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void SetCaster(Transform currCaster ,bool isAttached = false)
    {
        caster = currCaster;
        cRigidbody  = caster.GetComponent<Rigidbody2D>();
        cRenderer = caster.GetComponent<SpriteRenderer>();
        
        isCasterAttached = isAttached;
        
        if (!isCasterAttached) return;
        // 플레이어 비/활성화가 잠시 필요 - agent 쪽에서 인식 처리만 잘되면 됨
        cRenderer.enabled = false;
    }
    
    public void ExecuteSkill(SummonSkillManager.Skill skillName)
    {
        var (enemy, node) = SummonSkillManager.skills[skillName];
        animationHandler.SetController(EnemiesAnimator.animators[enemy.ToString()]);

        // bug: 한번 실행 후 마지막 start가 진행되는 것으로 보임
        // notice: 머신도 제거되는 지 체크 후 이벤트 제거 필요
        // notice: 오브젝트 풀링으로 인해 비활성화가 나을 수도 있음
        machine.OnLooped += () => Destroy(gameObject);
        machine.Define(node);
    }

    public void CancelAttached()
    {
        isCasterAttached = false;
        cRigidbody.velocity = Vector2.zero;
        cRenderer.enabled = true;
    }
    
    private void Update()
    {
        if (!isCasterAttached) return;
        caster.transform.position = transform.position;
    }

    private void OnDestroy()
    {
        if (!isCasterAttached) return;
        cRigidbody.velocity = Vector2.zero;
        cRenderer.enabled = true;
    }

    private void OnMove(InputValue value)
    {
        playerDirection = value.Get<Vector2>().normalized;
    }
}