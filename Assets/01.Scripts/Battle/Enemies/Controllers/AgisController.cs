using UnityEngine;

public class AgisController: EnemyController
{
    public override void Start()
    {
        btMachine.Define(
            new SelectorNode(new SequenceNode(new HitNode()), 
            new SequenceNode(new FlyingPatrolAttackNode(duration: 1f), new ParallelShotNode()))
        );
        
        base.Start(); // notice: 중복 발생하는 부분에 대한 처리 생각해보기
    }
}