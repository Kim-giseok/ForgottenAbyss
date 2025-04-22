using UnityEngine;

public class BoltLinearNode : BoltNode
{
    public override void Start()
    {
    }

    public override void Update()
    {
        // 비율 개념으로 가져는 것도 좋을 듯
        if (time >= 0.4f)
        {
            Next();
        }
        bolt.rigidbody.velocity = bolt.currDirection * bolt.speed;
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

public class RandomSpreadNode : BoltNode
{
    public override void Start()
    {
        bolt.rigidbody.drag = 10;
        bolt.rigidbody.AddForce(new Vector2(Random.Range(-4f, 4f), 4f) * 10f, ForceMode2D.Impulse);
    }

    public override void Update()
    {
        if (time >= 0.4f) { Next(); }
    }

    public override void End()
    {
        bolt.rigidbody.drag = 0;
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

public class BoltDecrescendoNode : BoltNode
{
    public override void Start()
    {
        // controller.rigidbody.AddForce(, ForceMode2D.Impulse);
    }
}

public class BoltRecursiveNode : BoltNode {}

public class BlackHoleNode : BoltNode
{
    public override void Start() {}

    public override void Update()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
    }
    
}