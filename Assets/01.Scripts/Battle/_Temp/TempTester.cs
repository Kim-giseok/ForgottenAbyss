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
      RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
      if (!hit.collider) return;
      
      Vector3 hitPoint = hit.point;
      
      // hit point의 모서리 충돌로 인한 한칸 보정 필요
      var cellPos = NavSurface.tilemap.WorldToCell(hitPoint);
      var curTile = NavSurface.cells.Find(cell => cell.tilePos == new Vector3Int(cellPos.x, cellPos.y - 1, 0));
      
      if (curTile != null) { Debug.Log(curTile.platformID); }
   }
   
   // private void Update()
   // {
   //    if (Input.GetKeyDown(KeyCode.K))
   //    {
   //       // direction 정보를 넘길 수 없음
   //       ProjectileManager.Instance.CreateSummon(transform, SummonSkillManager.Skill.BossSkill3, false);
   //    }
   //    
   //    if (Input.GetKeyDown(KeyCode.L))
   //    {
   //       ProjectileManager.Instance.CreateSummon(transform, SummonSkillManager.Skill.DashAttack, true);
   //    }
   // }

}