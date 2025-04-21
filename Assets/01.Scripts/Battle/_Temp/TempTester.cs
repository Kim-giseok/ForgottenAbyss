using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class TempTester: MonoBehaviour
{
   private BoxCollider2D collider;
   // 그냥 싱글톤으로 해도 될 듯
   public NavSurface NavSurface;
   private void Awake()
   {
      collider = GetComponent<BoxCollider2D>();
   }
   
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.K))
      {
         // direction 정보를 넘길 수 없음
         ProjectileManager.Instance.CreateSummon(transform, SummonSkillManager.Skill.BossSkill3, false);
      }
      
      if (Input.GetKeyDown(KeyCode.L))
      {
         ProjectileManager.Instance.CreateSummon(transform, SummonSkillManager.Skill.DashAttack, true);
      }
   }

}