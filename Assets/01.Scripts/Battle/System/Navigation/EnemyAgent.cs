using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

// 좌우 값을 기준으로 이동 가능한지 아닌지 우선 체크
public class EnemyAgent : MonoBehaviour
{
    public GameObject target { get; private set; }
    
    public float detectedDistance;
    public float stoppingDistance;
    public float tracingSpeed;
    
    // public Tilemap tilemap; // 추후 동적으로 찾도록 처리

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player"); // 만약 서먼이 몬스터를 향한다면?

        // agent에서 체크할 수 없으니 surface에서 등록 필요
        // BoundsInt bounds = tilemap.cellBounds;
        // TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        // foreach (Vector3Int position in bounds.allPositionsWithin)
        // {
        //     TileBase tile = tilemap.GetTile(position);
        //
        //     if (tile)
        //     {
        //         Vector3Int abovePosition = position + Vector3Int.up;
        //         TileBase tileAbove = tilemap.GetTile(abovePosition);
        //         if (tileAbove)
        //         {
        //         }
        //     }
        // }
    }

    public float GetDistance()
    {
        return (transform.position - target.transform.position).magnitude;
    }
}
