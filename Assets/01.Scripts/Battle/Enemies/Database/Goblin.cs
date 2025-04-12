public class Goblin
{
    public static Node node = new SelectorNode(
        new SequenceNode(new IdleNode(1), new AttackNode()));
}