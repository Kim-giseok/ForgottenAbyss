using UnityEngine;

public class LinearBolt : BoltNode
{
    public override void Update()
    {
        if (time >= 0.4f) Next();
        bolt.rigidbody.velocity = bolt.direction * bolt.speed;
    }
}

public class RainBolt : BoltNode
{
    public override void Start()
    {
        bolt.rigidbody.drag = 20;
        bolt.rigidbody.gravityScale = 32f;
        bolt.rigidbody.AddForce(new Vector2(Random.Range(-8f, 8f), 4f) * 40f, ForceMode2D.Impulse);
    }

    public override void Update() { if (time >= 0.4f) Next(); }

    public override void End()
    {
        bolt.rigidbody.drag = 0;
        bolt.rigidbody.gravityScale = 0f;
        bolt.rigidbody.AddForce(Vector2.down * 40f, ForceMode2D.Impulse);
    }
}

public class BoltGuidedNode : BoltNode
{
    public override void Update()
    {
        //     var currDegree = ProjectileManager.GetDegreeByDirection((target.position - transform.position).normalized);
        //     var nextRotation = Quaternion.Euler(0, 0 , currDegree - 90);
        //     transform.rotation = Quaternion.Slerp(transform.rotation, nextRotation, Time.deltaTime * 2f);
    }
}

public class BoldParabolaNode : BoltNode
{
    public override void Start()
    {
        // projectile.rigidbody.gravityScale = 1f;
        // projectile.rigidbody.velocity = Vector2.zero;
        // projectile.rigidbody.AddForce(direction * power, ForceMode2D.Impulse);
    }
}

public class BoltWaitNode : BoltNode {}

public class BoltCrescendoNode : BoltNode {}

public class DecrescendoBolt : BoltNode
{
    public override void Start()
    {
        bolt.rigidbody.velocity = Vector2.zero;
        bolt.rigidbody.drag = 8f;
        bolt.rigidbody.AddForce(bolt.direction * 24f, ForceMode2D.Impulse);
    }
    public override void Update() { if (time >= 0.2f) Next(); }
    public override void End() { bolt.rigidbody.drag = 0; }
}

public class RecursiveBolt : BoltNode
{
    public override void Start()
    {
        bolt.animHandler.Play("Heal");
        bolt.transform.position += (new Vector3(bolt.direction.x, bolt.direction.y, 0));
    }

    public override void Update() { if (time >= 0.2f) Next(); }
}

public class BlackHoleBolt : BoltNode
{
    // 앞으로 발사를 조금 넣어두는 게 좋을 듯
    public override void Start()
    {
        bolt.rigidbody.drag = 10;
        bolt.rigidbody.AddForce(new Vector2(Random.Range(-4f, 4f), 4f) * 10f, ForceMode2D.Impulse);
    }

    public override void Update()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        
        var direction = (bolt.transform.position - player.transform.position).normalized;
        player.GetComponent<Rigidbody2D>().AddForce(direction * 10f, ForceMode2D.Force);
    }
}

public class LaserBolt : BoltNode
{
    public override void Start()
    {
        LineRenderer renderer = bolt.lineRenderer;

        renderer.enabled = true;
        renderer.material.color = Color.magenta;
        
        renderer.startWidth = 0.05f;
        renderer.endWidth = 0.05f; 
        
        renderer.positionCount = 2;
        renderer.SetPosition(0, bolt.transform.position);
        renderer.SetPosition(1, bolt.transform.position + (Vector3)bolt.direction * 10f);
    }

    public override void Update()
    {
        LineRenderer renderer = bolt.lineRenderer;
        renderer.SetPosition(1, bolt.transform.position + (Vector3)bolt.direction * 10f);
    }
}

public class HealBolt : BoltNode { }
