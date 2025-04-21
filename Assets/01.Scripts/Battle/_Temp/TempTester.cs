using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class TempTester: MonoBehaviour
{
   private BoxCollider2D collider;
   // 그냥 싱글톤으로 해도 될 듯
   private void Awake()
   {
      collider = GetComponent<BoxCollider2D>();
   }
   
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.L))
      {
         BoltManager.Instance.CreateSummon(transform, SummonSkillManager.Skill.DashAttack, true);
      }
   }

}