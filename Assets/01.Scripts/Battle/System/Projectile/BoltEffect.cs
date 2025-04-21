using UnityEngine;

public abstract class BoltEffect
{
    private BoltController _boltController;
    public void Connect(BoltController boltController) => this._boltController = boltController;
    public abstract void Execute(Collider2D other);
}

public class BoltReflectionEffect : BoltEffect // 횟수는 어디서 관리?
{
    public override void Execute(Collider2D other)
    {
    }
}

public class BoltPiercingEffect : BoltEffect
{
    public override void Execute(Collider2D other)
    {
    }
}