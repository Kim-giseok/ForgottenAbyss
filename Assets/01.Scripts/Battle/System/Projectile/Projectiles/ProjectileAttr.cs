using UnityEngine;

public abstract class ProjectileAttr // 그냥 monoBehaviour로 프리팹화 시키는 편이 나을 수 있음
{
    protected Projectile projectile;

    public void Connect(Projectile projectile)
    {
        this.projectile = projectile;
    }

    public virtual void Start() {}
    public virtual void Update() {}
    public virtual void TriggerEnter(Collider2D other) {}
}

public class StraightAttr : ProjectileAttr
{
    public override void Update()
    {
        projectile.rigidbody.velocity = projectile.transform.up * projectile.speed;
    }
}

public class GuidedAttr : ProjectileAttr
{
    public override void Update()
    {
        // 유도 기능
        var currDegree = ProjectileManager.Instance.GetDegreeByDirection((projectile.target.position - projectile.transform.position).normalized);
        var nextRotation = Quaternion.Euler(0, 0 , currDegree - 90);
        projectile.transform.rotation = Quaternion.Slerp(projectile.transform.rotation, nextRotation, Time.deltaTime * 2f);
        
        projectile.rigidbody.velocity = projectile.transform.up * projectile.speed;
    }
}

public class ReflectAttr : ProjectileAttr
{
    private Vector2 reflectDir;

    public ReflectAttr()
    {
        reflectDir = projectile.transform.up;
    }
    
    public override void Start()
    {
    }

    public override void Update()
    {
        projectile.rigidbody.velocity = reflectDir * projectile.speed;
    }
    
    public override void TriggerEnter(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) return; // 수정 필요
        
        Vector3 contactPoint = other.ClosestPoint(projectile.transform.position);
        Vector3 normal = (projectile.transform.position - contactPoint).normalized;

        Vector3 reflected = Vector3.Reflect(projectile.rigidbody.velocity.normalized, normal);
        reflectDir= reflected;
    }
}

public class ParabolaAttr : ProjectileAttr
{
    private Vector2 direction;
    private float power;

    public ParabolaAttr(Vector2 dir, float power)
    {
        direction = dir;
        this.power = power;
    }

    public override void Start()
    {
        projectile.rigidbody.gravityScale = 1f;
        projectile.rigidbody.velocity = Vector2.zero;
        projectile.rigidbody.AddForce(direction * power, ForceMode2D.Impulse);
    }
}

public class ExplosionAttr: ProjectileAttr
{
    private float explusionTime;
    
    public ExplosionAttr(float explusionTime) { this.explusionTime = explusionTime; }

    public override void Start()
    {
        
    }
}

// 일정시간 혹은 충돌하면 재귀로 재생성
public class RecursionAttr : ProjectileAttr
{
    
}