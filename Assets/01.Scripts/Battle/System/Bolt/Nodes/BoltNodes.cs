using UnityEngine;

public class BoltLinearNode : BoltNode
{
    public override void Update()
    {
        bolt.rigidbody.velocity = bolt.currDirection * bolt.speed;
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