using UnityEngine;

// 좌우 값을 기준으로 이동 가능한지 아닌지 우선 체크
public class EnemyAgent : MonoBehaviour
{
    private EnemyBaseController controller;
    public GameObject target { get; private set; }
    public Collider2D tCollider { get; private set; }
    
    public enum Status { None, Detected, Tracked }
    [HideInInspector] public Status status = Status.None;

    private float DetectedDistance => 
        controller is not EnemyController eController ? 0f : eController.Resource.Get(EnemyStatType.SightRange)?.value ?? 0f;

    private float StoppingDistance => 
        controller is not EnemyController eController ? 0f : eController.Resource.Get(EnemyStatType.AttackRange)?.value ?? 0f;

    private float BoundaryDistance => 
        controller is not EnemyController eController ? 0f : eController.Resource.Get(EnemyStatType.BoundaryRange)?.value ?? 0f;

    public float TracingSpeed => 
        controller is not EnemyController eController ? 0f : eController.Resource.Get(EnemyStatType.Speed)?.value ?? 0f;
    
    public float defenseDistance;

    public float combatDuration; // 전
    
    // public Tilemap tilemap; // 추후 동적으로 찾도록 처리

    private void Awake()
    {
        controller = GetComponent<EnemyBaseController>();
        
        // 레이어 추후 enum으로 관리하기
        if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
        { 
            target = GameObject.FindGameObjectWithTag("Player"); // 만약 서먼이 몬스터를 향한다면?
            tCollider = target.GetComponent<Collider2D>();
        }
    }

    private void Update()
    {
        // notice: summon에서 monster를 역으로 추적하는 경우 - 그냥 없도록 하기
        // if (gameObject.layer == LayerMask.NameToLayer("Player") && Physics2D.OverlapCircle(transform.position, 3f, LayerMask.GetMask("Enemy")) is var hit && hit)
        // {
        //     target = hit.gameObject;
        // }

        if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            float currDistance = GetDistance();

            if (currDistance < StoppingDistance)
            {
                SetStatus(Status.Tracked);
                return;
            }

            if (currDistance < DetectedDistance)
            {
                // do: 여기서 키는 것이 맞을까?
                if (!controller.Board.IsAttacking) { controller.Board.IsAttacking = true; }
                SetStatus(Status.Detected);
                return;
            }

            SetStatus(Status.None);
        }
    }
    
    private void SetStatus(Status status)
    {
        if (this.status == status) return;
        this.status = status;
     
        // error : 공격 중일 때는 바로 notify되면 안된다.
        controller.Machine.Notify();
    }


    private float GetDistance() // horizontal 체크만 필요할 수도 있음
    {
        if (!target) { return 0; }
        return (target.transform.position - transform.position).magnitude;
    }

    public Vector2 GetDirection()
    {
        return (tCollider.bounds.center - transform.position).normalized;
    }

    public float GetDegree()
    {
        Vector2 currDirection = (tCollider.bounds.center - transform.position).normalized;
        float degree = Mathf.Atan2(currDirection.y, currDirection.x) * Mathf.Rad2Deg;
        return degree;
    }
}
