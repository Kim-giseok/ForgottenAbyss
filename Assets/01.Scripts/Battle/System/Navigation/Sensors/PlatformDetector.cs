using UnityEngine;

// 그라운드 체크 동시에 하기
// 경사 각도도 계산하기
public class PlatformDetector: MonoBehaviour
{
    private GameObject target;
    private BoxCollider2D collider;
    
    private void Awake()
    {
        target = transform.parent.gameObject;
        collider = GetComponent<BoxCollider2D>();
    }

    // notice: 콜라이더가 맨처음에 인식 못하는 현상 발생 
    private void OnEnable()
    {
        collider.enabled = false;
        collider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Collider2D tilemapCollider) || collision.gameObject.layer != NavSurface.Instance.layerMask) return;

        // RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, NavSurface.Instance.layerMask);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));
        // Debug.Log(hit.collider.name);
        
        if (!hit.collider) return;
        
        Vector3 hitPoint = hit.point;
        
        var cellPos = NavSurface.Instance.tilemap.WorldToCell(hitPoint);
        var curTile = NavSurface.Instance.cells.Find(cell => cell.tilePos == new Vector3Int(cellPos.x, cellPos.y - 1, 0));
        
        if (curTile == null) return;

        NavSurface.Instance.targetPlatforms[target] = curTile.platformID;
    }
}