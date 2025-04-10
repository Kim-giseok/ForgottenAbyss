public class MushroomController: EnemyController
{
    public override void Start()
    {
        base.Start();
        btMachine.Define(
            new SelectorNode(
                new SelectorNode(
                    new SequenceNode(new HitNode()),
                    new SequenceNode(new RangeToTargetNode(), new CombatIdleNode(duration: 1f))))
        );
    }
}