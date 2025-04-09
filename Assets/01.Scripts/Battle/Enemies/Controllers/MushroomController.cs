public class MushroomController: EnemyController
{
    public override void Start()
    {
        btMachine.Define(
            new SelectorNode(
                new SelectorNode(
                    new SequenceNode(new HitNode()),
                    new SequenceNode(new RangeToTargetNode(), new CombatIdleNode(duration: 0.5f))))
        );
    }
}