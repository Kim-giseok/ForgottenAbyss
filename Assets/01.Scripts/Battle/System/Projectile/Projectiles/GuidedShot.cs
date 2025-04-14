using UnityEngine;

// do: 단순 일정 방향으로 회전하는 경우 필요 
// 브릿지 패턴으로 조합하도록
public class GuidedShot: MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private Transform target; // 만약 적이 타겟이라면? Layer로 체크하기

    private float currTime;
    public float duration;
    public float speed;
    
    
    // public override void Update()
    // {
    //     // 유도 기능
    //     var currDegree = ProjectileManager.Instance.GetDegreeByDirection((projectile.target.position - projectile.transform.position).normalized);
    //     var nextRotation = Quaternion.Euler(0, 0 , currDegree - 90);
    //     projectile.transform.rotation = Quaternion.Slerp(projectile.transform.rotation, nextRotation, Time.deltaTime * 2f);
    //     
    //     projectile.rigidbody.velocity = projectile.transform.up * projectile.speed;
    // }


    private void SetTarget()
    {
        if (!target)
        {
            // var hit = Physics2D.OverlapCircleAll(transform.position, 1f, targetLayer);
            // if (hit == null) return;
            
        }
        
        float degree = ProjectileManager.Instance.GetDegreeByDirection((target.position - transform.position).normalized);
        transform.localRotation = Quaternion.Euler(0, 0, degree);
    }
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        currTime = 0;
    }

    public void FixedUpdate()
    {
        // 일정 시간 이후 삭제
        currTime += Time.fixedDeltaTime;
        if (currTime >= duration) 
        {
            ProjectileManager.Instance.DestroyProjectile(gameObject);
            return;
        }

        // 유도 기능
        var currDegree = ProjectileManager.Instance.GetDegreeByDirection((target.position - transform.position).normalized);
        var nextRotation = Quaternion.Euler(0, 0 , currDegree - 90);
        transform.rotation = Quaternion.Slerp(transform.rotation, nextRotation, Time.deltaTime * 2f);


        // 발사 로직
        rigidbody.velocity = transform.up * speed;
    } 
}