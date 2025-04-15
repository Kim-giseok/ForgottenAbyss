using UnityEngine;

public class TempTester: MonoBehaviour
{

   private void Start()
   {
   }
   
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.K))
      {
         // direction 정보를 넘길 수 없음
         ProjectileManager.Instance.CreateSummonProjectile(transform, SummonSkillManager.Skill.BossSkill2, false);
         ProjectileManager.Instance.CreateSummonProjectile(transform, SummonSkillManager.Skill.BossSkill3, false);
      }
      
      if (Input.GetKeyDown(KeyCode.L))
      {
         ProjectileManager.Instance.CreateSummonProjectile(transform, SummonSkillManager.Skill.DashAttack, true);
      }
   }
}