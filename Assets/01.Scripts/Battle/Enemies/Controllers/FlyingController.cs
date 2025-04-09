public class FlyingController : EnemyController
{
    protected override void Init()
    {
        btMachine.Define(
            new SelectorNode(
                new SequenceNode(new HitNode()),
                new SequenceNode(new TracingNode(), new RangeAttackNode(), new CombatIdleNode(duration: 1f)),
                new SequenceNode(new IdleNode(duration: 1), new PatrolNode(duration: 1)))
        );
    }
}