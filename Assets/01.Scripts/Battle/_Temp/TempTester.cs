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
      if (Input.GetKeyDown(KeyCode.Alpha1))
      {
         BoltsPool.Instance.Create(transform, Bolts.Type.Linear)
            .SetSprite("arrow")
            .SetSpeed(20f)
            .SetSize(1f)
            .SetEffect(Bolts.EffectType.Reflection)
            .Fire();
      }

      if (Input.GetKeyDown(KeyCode.Alpha2))
      {
         StartCoroutine(FireBoltsSequentially(30, 0.1f));
      }

      if (Input.GetKeyDown(KeyCode.Alpha3))
      {
         BoltsPool.Instance.Create(transform, Bolts.Type.BlackHole).SetSize(4).Fire();
      }

      if (Input.GetKeyDown(KeyCode.Alpha4))
      {
         BoltsPool.Instance.CreateSummon(transform, SummonSkillManager.Skill.ComboDashAttack, true).Fire();
      }
      
      if (Input.GetKeyDown(KeyCode.Alpha5))
      {
         BoltsPool.Instance.CreateSummon(transform, SummonSkillManager.Skill.Heal, true).Fire();
      }

      if (Input.GetKeyDown(KeyCode.Alpha6))
      {
         BoltsPool.Instance.CreateSummon(transform, SummonSkillManager.Skill.ArcherArrow, true).Fire();
      }

      if (Input.GetKeyDown(KeyCode.Alpha7))
      {
         for (int i = 0; i < 9; i++)
         {
            float angle = i * 40 * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 32;
            BoltsPool.Instance.CreateSummon(transform, SummonSkillManager.Skill.Agis, true).SetCastingDirection(direction).Fire();
         }
      }

      if (Input.GetKeyDown(KeyCode.Alpha8))
      {
         foreach (Platform platform in NavSurface.Instance.platforms)
         {
            BoltsPool.Instance.CreateSummon(transform, SummonSkillManager.Skill.MudWave, false).SetPosition(platform.startCell.WorldPos + new Vector2(0, 1.5f)).Fire();
         }
         BoltsPool.Instance.CreateSummon(transform, SummonSkillManager.Skill.MudEye, true).Fire();
      }

      if (Input.GetKeyDown(KeyCode.Alpha9))
      {
         BoltsPool.Instance.CreateSummon(transform, SummonSkillManager.Skill.DashAttack, true).Fire();

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