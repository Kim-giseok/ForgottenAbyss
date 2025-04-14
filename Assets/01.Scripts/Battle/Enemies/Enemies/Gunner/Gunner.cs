using System.Collections.Generic;

public class Gunner: Enemy
{
    public enum Skill { DashAttack }

    public override Dictionary<string, Node> skills { get; protected set; } = new()
    {
        { Skill.DashAttack.ToString(), new MeleeAttack() }
    };

    public override Node Node { get; protected set; } = new SelectorNode(
        new SequenceNode(new HitNode()),
        // 공격 이후 쿨타임 1초 간격
        new SequenceNode(new TracingNode(), new RangeAttackNode(), new IdleNode(1)),
        new SequenceNode(new IdleNode(1), new PatrolMove(1))
    );
}