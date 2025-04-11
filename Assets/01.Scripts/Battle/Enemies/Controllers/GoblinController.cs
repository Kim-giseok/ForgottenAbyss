
using UnityEngine;

public class GoblinController: EnemyController
{
    public EnemySkillNodes skill = new();
    
    
    public override void Start()
    {
        // notice: 시작할 때 노드를 매번 만들 필요 없이 노드가 동일하다면 모든 고블린이 공유해도 될 둣(controller를 하나만 이용하고 dictionary 등을 통해서 추출하기
        // 메모리 스킬도 동적으로 생성한다는 것이 장점이긴 하지만 객체 생성 비용이 발생할 수 있는 점에 대해서 고려 필요
        // 그렇다면 behaviour tree를 JSON의 tree구조로 작성하거나 하는 등으로 재활용 가능할 수 있음
        Debug.LogWarning("Goblin controller start");
        btMachine.Define(
            new SelectorNode(
                new SequenceNode(new HitNode()),
                new SequenceNode(new TracingNode(),
                    skill.doubleAttack,
                    // new AttackNode(),
                    // new DashAttack(),
                    // new JumpAttack(),
                    new CombatIdleNode(duration: 0.5f)),
                new SequenceNode(new IdleNode(duration: 1), new PatrolNode(duration: 1)))
        );
        
        base.Start();
    }   
}