using UnityEngine;

public class ActorController: EnemyBaseController
{
    public SummonSkillManager.Animation currAnimation;
    private void Start()
    {
        Define(currAnimation);
        machine.Start();
    }

    private void Define(SummonSkillManager.Animation animationName)
    {
        var (_, node) = SummonSkillManager.Animations[(int)animationName];

        // 기본 노드 안에 waitNode하나를 둬서 다음 행동이 발생하기 전까지 기다리게 하기
        // 한텀 돌면 삭제가 아니라 잠시 정지
        machine.OnLooped += () =>
        {
            Debug.Log("end");
            // EnemyRespawner.Instance.Create(Enemies.Enemy.Agis, transform.position);
        };
        
        machine.Define(node);
    }
}