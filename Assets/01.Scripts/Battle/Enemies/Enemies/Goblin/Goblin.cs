using System.Collections.Generic;

public class Goblin: Enemy
{
    public enum Skill { DashAttack }

    public override Dictionary<string, Node> skills { get; protected set; } = new()
    {
        { Skill.DashAttack.ToString(), new AttackNode() }
    };

    // public override Node Node { get; protected set; } = new SelectorNode(
    //     new HitNode(),
    //     new SequenceNode(new IdleNode(0.5f), new PatrolMove(0.5f))
    //     );

    // public override Node Node { get; protected set; } = new SequenceNode(new IdleNode(1), new PatrolMove(1f));
    public override Node Node { get; protected set; } = new PatrolMove(3f);
    
    // public override Node Node { get; protected set; } = 
    //     new SelectorNode(
    //         new HitNode(),
    //         new ParallelNode(new NormalModeNode(), new SequenceNode(new IdleNode(1), new PatrolMove(1))),
    //         new ParallelNode(new CombatModeNode(), new SequenceNode(new AttackNode(), new IdleNode(0.5f)))
    //         );
}