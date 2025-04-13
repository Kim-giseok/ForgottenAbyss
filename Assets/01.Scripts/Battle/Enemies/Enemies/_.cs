using System.Collections.Generic;

// 한 스킬 액션 자체도 데이터화 한다면 비용이 더 줄 수 있음
public class _legacyBehaviours
{
    // 어떻게 관리해야할 시 생각 필요
    // enum을 Key할 경우 박싱 발생함
    public static Dictionary<string, Node> enemies = new() 
    {
        {"test", test}
    };

    public static Node doubleAttack = new SequenceNode(new AttackNode(), new AttackNode());
    public static Node MonsterNode = new SelectorNode(doubleAttack);
    
    public static Node test = new SelectorNode(new SequenceNode(new IdleNode(1), new AttackNode()));
    
    private Node agis = new SelectorNode(new SequenceNode(new HitNode()),
        new SequenceNode(new ParallelShotNode()));

    private Node goblin = new SelectorNode(
        new SequenceNode(new HitNode()),
        new SequenceNode(new TracingNode(),
            // skill.doubleAttack,
            new AttackNode(),
            // new DashAttack(),
            // new JumpAttack(),
            new IdleNode(duration: 0.5f)),
        new SequenceNode(new IdleNode(duration: 1), new PatrolMove(duration: 1)));

    private Node flying = new SelectorNode(new SequenceNode(new HitNode()));


    private Node mushroom = new SelectorNode(
        new SelectorNode(
            new SequenceNode(new HitNode()),
            new SequenceNode(new RangeToTargetNode(), new IdleNode(duration: 1f))));
}
