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

        machine.OnLooped += () =>
        {
            Debug.Log("end");
            // EnemyRespawner.Instance.Create(Enemies.Enemy.Agis, transform.position);
        };
        
        machine.Define(node);
    }
}