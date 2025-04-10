using UnityEngine;

public class TempTester: MonoBehaviour
{
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.K))
      {
         Debug.Log("pressed");
         ProjectileManager.Instance.CreateEnemyProjectile(transform, "DashAttack");
      }
   }
}