using UnityEngine;

public class TempTester: MonoBehaviour
{

   private void Start()
   {
      Debug.Log(Enemies.Enemy.NightBone.ToString());
   }
   
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.K))
      {
         // ProjectileManager.Instance.CreateEnemyProjectile(transform, "DashAttack", 0);
         // ProjectileManager.Instance.CreateEnemyProjectile(transform, "ParallelShotNode", 2);
      }
      
      if (Input.GetKeyDown(KeyCode.L))
      {
         ProjectileManager.Instance.CreateEnemyProjectile(transform, SummonSkillManager.Skill.DashAttack);
      }
   }
}