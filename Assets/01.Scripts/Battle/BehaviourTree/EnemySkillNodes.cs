// 싱글톤으로 쓴다면 비용 문제가 오히려 줄어들 수 있음, 아니면 각 몬스터 컨트롤러 안에서 실행?
public class EnemySkillNodes
{
    public Node doubleAttack = new SequenceNode(new AttackNode(), new AttackNode());
}