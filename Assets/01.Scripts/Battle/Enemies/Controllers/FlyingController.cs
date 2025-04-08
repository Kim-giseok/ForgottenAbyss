public class FlyingController: EnemyController
{
    public override void Start()
    {
        btMachine.Define(
            new SelectorNode(
                new SequenceNode(new HitNode()),
                new SequenceNode(new TracingNode(), new RangeAttackNode(), new CombatIdleNode(duration: 1f)),
                new SequenceNode(new IdleNode(duration: 1), new PatrolNode(duration: 1)))
        );
    }
}