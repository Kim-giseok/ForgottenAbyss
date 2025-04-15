using UnityEngine;

public class LinearShot: Projectile
{
    private float currTime;
    public float duration;
    public float speed;
    

    private void Awake()
    {
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
        
    }
}