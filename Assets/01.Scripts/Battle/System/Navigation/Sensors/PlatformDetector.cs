using System.Collections.Generic;
using UnityEngine;

// 그라운드 체크 동시에 하기
// 경사 각도도 계산하기
public class PlatformDetector: MonoBehaviour
{
    private GameObject target;
    private Collider2D tCollider;
    private Vector2 tRayPoint = Vector2.zero;

    private BoxCollider2D _collider;
    
    private void Awake()
    {
        target = transform.parent.gameObject;
        _collider = GetComponent<BoxCollider2D>();
        tCollider = GetComponentInParent<Collider2D>();
    }

    // notice: 콜라이더가 맨처음에 인식 못하는 현상 발생 
    private void OnEnable()
    {
        _collider.enabled = false;
        _collider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out Collider2D tilemapCollider) || other.gameObject.layer != NavSurface.Instance.layerMask) return;

        // 플레이어는 transform.position이 바닥이지만 일반적인 경우 중앙부터 - 수정 필요
        tRayPoint.x = tCollider.bounds.center.x; tRayPoint.y = tCollider.bounds.min.y;
        RaycastHit2D hit = Physics2D.Raycast(tRayPoint, Vector2.down, 0.2f, LayerMask.GetMask("Ground"));
        
        if (!hit.collider) return;
        
        Vector3 hitPoint = hit.point;
        
        // bug: 정수 변환으로 인해 위치 값이 일치하지 않는 경우 발생 
        var cellPos = NavSurface.Instance.tilemap.WorldToCell(hitPoint);
        var curTile = NavSurface.Instance.cells.Find(cell => cell.tilePos.x == cellPos.x && cell.tilePos.y == cellPos.y);
        
        if (curTile == null) return;
        
        NavSurface.Instance.targetPlatforms[target] = curTile.platformID;
    }
}