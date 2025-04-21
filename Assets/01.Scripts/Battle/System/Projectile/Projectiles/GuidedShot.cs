using UnityEngine;

// do: 단순 일정 방향으로 회전하는 경우 필요 
// 브릿지 패턴으로 조합하도록
public class GuidedShot: Bolt
{
    // public Transform target;
    //
    // private void SetTarget()
    // {
    //     if (!target)
    //     {
    //         // var hit = Physics2D.OverlapCircleAll(transform.position, 1f, targetLayer);
    //         // if (hit == null) return;
    //         
    //     }
    //     
    //     float degree = ProjectileManager.GetDegreeByDirection((target.position - transform.position).normalized);
    //     transform.localRotation = Quaternion.Euler(0, 0, degree);
    // }
    //
    // private void Awake()
    // {
    //     rigidbody = GetComponent<Rigidbody2D>();
    //     target = GameObject.FindGameObjectWithTag("Player").transform;
    // }
    //
    // protected override void FixedUpdate()
    // {
    //     base.FixedUpdate();
    //     // 일정 시간 이후 삭제
    //     if (currTime >= duration) 
    //     {
    //         ProjectileManager.Instance.DestroyProjectile(gameObject);
    //         return;
    //     }
    //
    //     // 유도 기능
    //     var currDegree = ProjectileManager.GetDegreeByDirection((target.position - transform.position).normalized);
    //     var nextRotation = Quaternion.Euler(0, 0 , currDegree - 90);
    //     transform.rotation = Quaternion.Slerp(transform.rotation, nextRotation, Time.deltaTime * 2f);
    //
    //
    //     // 발사 로직
    //     rigidbody.velocity = transform.up * speed;
    // } 
}