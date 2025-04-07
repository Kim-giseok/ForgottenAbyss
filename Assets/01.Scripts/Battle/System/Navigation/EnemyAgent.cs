using UnityEngine;
using UnityEngine.Tilemaps;

// 좌우 값을 기준으로 이동 가능한지 아닌지 우선 체크
public class EnemyAgent : MonoBehaviour
{
    [HideInInspector] public Transform player { get; private set; }
    public Tilemap tilemap; // 추후 동적으로 찾도록 처리

    private void Awake()
    {
        BoundsInt bounds = tilemap.cellBounds;
        Debug.Log(tilemap.GetTilesBlock(bounds).Length); // 혹은 내 캐릭 주변으로 감지
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
