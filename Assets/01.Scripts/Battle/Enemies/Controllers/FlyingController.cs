public class FlyingController : EnemyController
{
    public override void Start()
    {
        btMachine.Define(
                new SelectorNode(new SequenceNode(new HitNode()), 
                new SequenceNode(new FlyingIdleNode(duration: 1f), new FlyingPatrolAttackNode(duration: 1f)))
            );
        
        base.Start();
    }
}