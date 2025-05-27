using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

// feat: 플레이어 인풋 연결
// pController 가 액터가 없는 경우 확인 필요
public class SummonController: EnemyBaseController
{
    // 발사 직전 캡처링 형태로 정보 등록
    public int hitInformation; // 현재 공격에 대한 정보를 저장받는다.
    public Vector2 castingDirection;
    // public EnemyStatusHandler statusHandler {get; private set;} 

    public Transform caster {get; private set;}
    public bool isPlayerCaster { get; private set; } = false;
    
    public ControllerPlayer pController { get; private set; }
    public EnemyController eController { get; private set; }
    
    private bool isCasterAttached = true;

    public Rigidbody2D cRigidbody {get; private set;}
    private Collider2D cCollider;
    private SpriteRenderer cRenderer;

    public float degree;
    public static Vector2 InputDirection => GameManager.Instance.player.controller.inputVec;
    
    public SummonController SetCastingDirection(Vector2 direction)
    {
        this.castingDirection = direction;
        return this;
    }

    public SummonController SetPosition(Vector2 position)
    {
        this.transform.position = position;
        return this;
    }

    public SummonController SetTrigger(bool isTrigger)
    {
        Collider.isTrigger = isTrigger;
        return this;
    }

    public SummonController Fire()
    {
        Machine.Start();
        return this;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void SetCaster(Transform currCaster ,bool isAttached = false)
    {
        this.eController = null;
        this.pController = null;
        
        // 캐스팅마다 가져오면서 비용이 커질 수 있는 점 관리 필요 - 캐싱을 통해서 
        caster = currCaster;
        // 컨트롤러만 가져오면 내부에서 파악할 수 있다.
        cRigidbody  = caster.GetComponent<Rigidbody2D>();
        cRenderer = caster.GetComponent<SpriteRenderer>();

        if (caster.TryGetComponent(out ControllerPlayer pController))
        {
            this.pController = pController;
            isPlayerCaster = true;
        }

        if (caster.TryGetComponent(out EnemyController eController))
        {
            this.eController = eController;
            // 주입은 외부에서 가능하게 하고, 다양한 플레이어가 자신 만의 값으로 등록하도록 변경하기
            // statusHandler인 경우 깊은 복사가 필요해질 수 있음
            castingDirection = eController.Status.castingDirection;
            isPlayerCaster = false;
        }
        
        isCasterAttached = isAttached;
        
        if (!isCasterAttached) return;
        SkillController.Instance.isDisable = true;
        // 플레이어 비/활성화가 잠시 필요 - agent 쪽에서 인식 처리만 잘되면 됨
        cRenderer.enabled = false;
        // 서먼 스킬을 사용하는 동안은 무적 처리
        if(pController) { pController.isInvincible = true; } 
    }
    
    public void Define(SummonSkillManager.Skill skillName)
    {
        var (enemy, node) = SummonSkillManager.skills[(int)skillName];
        Anim.SetController(EnemiesAnimator.animators[enemy.ToString()]);

        // bug: 한번 실행 후 마지막 start가 진행되는 것으로 보임
        // notice: 머신도 제거되는 지 체크 후 이벤트 제거 필요
        // notice: 오브젝트 풀링으로 인해 비활성화가 나을 수도 있음 - 적용하기
        Machine.OnLooped += () => Destroy(gameObject);
        Machine.Define(node);
        
        // notice: 사이즈 자동 지정 기능
        var info = EnemiesLoader.EnemiesInfoSO.EnemyViewInfos.Find(info => info.enemyName == enemy.ToString());
        if (info == null) return;
        
        transform.localScale = new Vector2(info.ratio, info.ratio);
        Collider.size = info.size;
        Collider.offset = new Vector2(0, info.size.y / 2);
    }

    public void CancelAttached()
    {
        isCasterAttached = false;
        cRigidbody.velocity = Vector2.zero;
        cRenderer.enabled = true;
        SkillController.Instance.isDisable = false;

        if(pController) { pController.isInvincible = false; }
    }

    private void Start()
    {
        Render.material.SetFloat("_YValue", 0);
        StartCoroutine(PlaySpawnAnimation(3f));
    }

    private IEnumerator PlaySpawnAnimation(float speed)
    {
        float currentYValue = 0f;
        while (currentYValue < 1f)
        {
            currentYValue = Render.material.GetFloat("_YValue");
            float newYValue = Mathf.MoveTowards(currentYValue, 1f, Time.deltaTime * speed);
            Render.material.SetFloat("_YValue", newYValue);
            yield return null;
        }
        
        // agis의 경우 변경되지 않는 점 확인 필요
        // renderer.color = Color.black;
    }
    
    private void Update()
    {
        // 다른 방식으로 처리 필요
        // if (Input.GetKeyDown(KeyCode.A)) { Machine.currNode.OnPressed(); } // 임시 등록

        if (isCasterAttached)
        {
            caster.transform.position = transform.position;
        }
    }

    protected void OnDestroy()
    {
        if (!isCasterAttached) return;
        cRigidbody.velocity = Vector2.zero;
        cRenderer.enabled = true;
        Collider.isTrigger = false;
        SkillController.Instance.isDisable = false;
        if(pController) { pController.isInvincible = false; }
    }
}