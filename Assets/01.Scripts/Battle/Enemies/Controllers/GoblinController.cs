
public class GoblinController: EnemyController
{
    public override void Start()
    {
        btMachine.Define(
            new SelectorNode(
                new SequenceNode(new HitNode()),
                new SequenceNode(new TracingNode(), 
                    // new AttackNode(),
                    // new DashAttack(),
                    new JumpAttack(),
                    new CombatIdleNode(duration: 2f)),
                new SequenceNode(new IdleNode(duration: 1), new PatrolNode(duration: 1)))
        );
        
        base.Start();
    }   
}