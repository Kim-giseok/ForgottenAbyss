using System.Collections.Generic;

public class Goblin: Enemy
{
    public override Dictionary<string, Node> skills { get; protected set; } = new()
    {
        { "DashAttack", new AttackNode() }
    };

    public override Node Node { get; protected set; } = 
        new SelectorNode(
            new HitNode(),
            new SequenceNode(new AttackNode(), new PatrolMove(1)));
}