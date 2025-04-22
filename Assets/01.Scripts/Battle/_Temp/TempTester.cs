using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class TempTester: MonoBehaviour
{
   private BoxCollider2D collider;

   private float timer;
   // 그냥 싱글톤으로 해도 될 듯
   private void Awake()
   {
      collider = GetComponent<BoxCollider2D>();
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.L))
      {
         // BoltManager.Instance.CreateSummon(transform, SummonSkillManager.Skill.DashAttack, true);
         BoltsPool.Instance.CreateSummon(transform, SummonSkillManager.Skill.ComboDashAttack, true);
      }

      if (Input.GetKeyDown(KeyCode.K))
      {
         // for(int index = 0; index < 5; index++) {BoltsPool.Instance.Create(transform, Bolts.Type.Test);}
         BoltsPool.Instance.Create(transform, Bolts.Type.BlackHole).SetDirection(transform.right).Fire();
      }
   }

}