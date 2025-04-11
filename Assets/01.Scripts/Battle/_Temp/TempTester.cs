using UnityEngine;

public class TempTester: MonoBehaviour
{
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.K))
      {
         // ProjectileManager.Instance.CreateEnemyProjectile(transform, "DashAttack", 0);
         ProjectileManager.Instance.CreateEnemyProjectile(transform, "ParallelShotNode", 2);
      }
      
      if (Input.GetKeyDown(KeyCode.L))
      {
         ProjectileManager.Instance.CreateEnemyProjectile(transform, "DashAttack", 0);
      }
      
      if (Input.GetKeyDown(KeyCode.J))
      {
         ProjectileManager.Instance.CreateMeleeProjectile(transform, 10, startPos: Vector2.zero, size: new Vector2(3f, 3f));
      }

      if (Input.GetKeyUp(KeyCode.J))
      {
         
         ProjectileManager.Instance.DestroyMeleeProjectile(transform);
      }
   }
}