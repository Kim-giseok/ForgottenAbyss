using System.Collections;
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
         BoltsPool.Instance.Create(transform, Bolts.Type.Linear)
            .SetSprite("arrow")
            .SetSpeed(20f)
            .SetSize(1f)
            .SetEffect(Bolts.EffectType.Reflection)
            .Fire();
      }

      if (Input.GetKeyDown(KeyCode.M))
      {
         StartCoroutine(FireBoltsSequentially(30, 0.1f));
      }

      if (Input.GetKeyDown(KeyCode.J))
      {
         BoltsPool.Instance.Create(transform, Bolts.Type.BlackHole).Fire();
      }
   }

   private IEnumerator FireBoltsSequentially(int count, float delay)
   {
      for (int i = 0; i < count; i++)
      {
         BoltsPool.Instance.Create(transform, Bolts.Type.Rain).Fire();
         yield return new WaitForSeconds(delay);
      }
   }
}