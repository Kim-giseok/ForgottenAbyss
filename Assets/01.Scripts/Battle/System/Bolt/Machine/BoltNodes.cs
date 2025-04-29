using UnityEngine;

public class LinearBolt : BoltNode
{
    public override void Update()
    {
        if (currTime >= 0.4f) Next();
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

    public override void Update() { if (currTime >= 0.4f) Next(); }

    public override void End()
    {
        // bolt.trailRenderer.enabled = true;
        
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
        bolt.rigidbody.AddForce(bolt.direction * bolt.speed, ForceMode2D.Impulse);
    }
    public override void Update() { if (currTime >= 0.2f) Next(); }
    public override void End() { bolt.rigidbody.drag = 0; }
}

public class RecursiveBolt : BoltNode
{
    public override void Start()
    {
        bolt.animHandler.Play("Soul");
        
        // 플레이어 위치 의존적인 부분 처리 필요
        // bolt.transform.position += (new Vector3(bolt.direction.x, bolt.direction.y, 0));
        Vector2 direction = (GameManager.Instance.player.transform.position - bolt.transform.position).normalized;
        bolt.transform.position += (new Vector3(direction.x, direction.y, 0));
    }

    public override void Update() { if (currTime >= 0.2f) Next(); }
}

public class BlackHoleBolt : BoltNode
{
    // 앞으로 발사를 조금 넣어두는 게 좋을 듯
    public override void Start()
    {
        bolt.SetSize(4);
        bolt.animHandler.Play("BlackHole");
        bolt.renderer.color = Color.black;
        
        bolt.rigidbody.drag = 2;
        
        Debug.Log(bolt.direction);
        bolt.rigidbody.AddForce(Vector2.down * 12f, ForceMode2D.Impulse);
    }

    public override void Update()
    {
        // RaycastHit2D[] hits = Physics2D.CircleCastAll(bolt.transform.position, 4f, Vector2.down,  LayerMask.GetMask("Player", "Enemy"));
        RaycastHit2D[] hits = Physics2D.CircleCastAll(bolt.transform.position, 4f, Vector2.down, 12f, LayerMask.GetMask("Player"));
        foreach (var hit in hits)
        {
            if (!hit.rigidbody) continue;
            var direction = (bolt.transform.position - hit.transform.position).normalized;
            hit.rigidbody.AddForce(direction * 4f, ForceMode2D.Force);
        }
    }

    public override void End()
    {
        bolt.animHandler.Play("None");
        bolt.rigidbody.drag = 0;
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

public class HealBolt : BoltNode
{
    public override void Start()
    {
        bolt.SetSize(1f);
        bolt.animHandler.Play("Heal");
    }
}

public class ForwardBolt : BoltNode
{
    public override void Start()
    {
        bolt.collider.enabled = false;
    }

    public override void Update()
    {
        
        var currPos = bolt.transform.position;
        if (Mathf.Abs(currPos.z - 0) < 1f)
        {
            bolt.collider.enabled = true;
        }
        else
        {
            bolt.collider.enabled = false;
        }

        // 왜인지 모르지만 잘 맞는다
        currPos.x += bolt.direction.x / 100;
        currPos.y += bolt.direction.y / 100;
        currPos.z -= 0.1f;
        
        bolt.collider.transform.position = currPos;
    }
}