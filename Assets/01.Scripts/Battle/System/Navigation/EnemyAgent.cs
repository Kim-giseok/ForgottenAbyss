using UnityEngine;

// 좌우 값을 기준으로 이동 가능한지 아닌지 우선 체크
public class EnemyAgent : MonoBehaviour
{
    private EnemyBaseController controller;
    public GameObject target { get; private set; }
    
    public enum Status { None, Detected, Tracked }
    [HideInInspector] public Status status = Status.None;
    
    public float detectedDistance;
    public float stoppingDistance;
    public float tracingSpeed;
    
    // public Tilemap tilemap; // 추후 동적으로 찾도록 처리

    private void Awake()
    {
        controller = GetComponent<EnemyBaseController>();
        
        // 레이어 추후 enum으로 관리하기
        if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
        { 
            target = GameObject.FindGameObjectWithTag("Player"); // 만약 서먼이 몬스터를 향한다면?
        }
    }

    private void Update()
    {
        // notice: summon에서 monster를 역으로 추적하는 경우
        if (gameObject.layer == LayerMask.NameToLayer("Player") && Physics2D.OverlapCircle(transform.position, 3f, LayerMask.GetMask("Enemy")) is var hit && hit)
        {
            target = hit.gameObject;
        }


        if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            float currDistance = GetDistance();
            
            if (currDistance < stoppingDistance) { SetStatus(Status.Tracked); return; }
            if (currDistance < detectedDistance) { SetStatus(Status.Detected); return; }
            SetStatus(Status.None);
        }
    }
    
    private void SetStatus(Status status)
    {
        if (this.status == status) return;
        this.status = status;
        
        controller.OnAgentDetected(status);
    }
    

    public float GetDistance() // horizontal 체크만 필요할 수도 있음
    {
        if (!target) return 0;
        return (target.transform.position - transform.position).magnitude;
    }

    public Vector2 GetDirection()
    {
        if (!target) return GameObject.FindWithTag("Player").transform.right; // enemy와 summon이 공통으로 사용하면서 문제가 발생함
        return (target.transform.position - transform.position).normalized;
    }
}
