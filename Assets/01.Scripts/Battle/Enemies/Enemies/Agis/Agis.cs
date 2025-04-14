
using System.Collections.Generic;

public class Agis: Enemy
{
    public enum Skill { DashAttack }

    public override Dictionary<string, Node> skills { get; protected set; } = new()
    {
        { Skill.DashAttack.ToString(), new AttackNode() }
    };

    public override Node Node { get; protected set; } = new SelectorNode(
        new SequenceNode(new HitNode()),
        new SequenceNode(new TracingNode(), new AttackNode()),
        new SequenceNode(new IdleNode(1), new PatrolMove(1))
    );
}