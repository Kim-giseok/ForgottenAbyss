using UnityEngine;

public class GuidedProjectile: MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private Transform target;

    private float currTime;
    public float duration;
    public float speed;


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
        currTime += Time.fixedDeltaTime;
        if (currTime >= duration)
        {
            ProjectileManager.Instance.DestroyProjectile(gameObject);
            return;
        }

        var currDegree = ProjectileManager.Instance.GetDegreeByDirection((target.position - transform.position).normalized);
        var nextRotation = Quaternion.Euler(0, 0 , currDegree - 90);
        transform.rotation = Quaternion.Slerp(transform.rotation, nextRotation, Time.deltaTime * 2f);


        rigidbody.velocity = transform.up * speed;
    } 
}