public class FlyingController : EnemyController
{
    protected override void Init()
    {
        btMachine.Define(
                new SelectorNode(new SequenceNode(new HitNode()), 
                new SequenceNode(new FlyingIdleNode(duration: 1f), new FlyingPatrolAttackNode(duration: 1f)))
            );
    }
}