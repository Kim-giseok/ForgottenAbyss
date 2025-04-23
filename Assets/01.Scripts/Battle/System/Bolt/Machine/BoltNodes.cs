using UnityEngine;

public class LinearBolt : BoltNode
{
    public override void Start()
    {
        Debug.Log(bolt.direction);
    }

    public override void Update()
    {
        if (time >= 0.4f) Next();
        bolt.rigidbody.velocity = bolt.direction * bolt.speed;
    }
}

public class BoltTestDownNode : BoltNode
{
    public override void Start()
    {
        bolt.rigidbody.velocity = Vector2.zero;
    }
    public override void Update()
    {
        bolt.rigidbody.velocity = Vector2.down * bolt.speed;
    }
}

public class RandomSpreadBolt : BoltNode
{
    public override void Start()
    {
        bolt.rigidbody.drag = 10;
        bolt.rigidbody.AddForce(new Vector2(Random.Range(-4f, 4f), 4f) * 10f, ForceMode2D.Impulse);
    }

    public override void Update() { if (time >= 0.4f) Next(); }

    public override void End() { bolt.rigidbody.drag = 0; }
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

public class RecursiveBolt : BoltNode {}

public class BlackHoleBolt : BoltNode
{
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

public class HealBolt : BoltNode
{
    
}