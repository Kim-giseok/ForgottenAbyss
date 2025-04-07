using UnityEngine;
using UnityEngine.Tilemaps;

public class MonsterAgent : MonoBehaviour
{
    private Transform player;
    public Tilemap tilemap; // 추후 동적으로 찾도록 처리

    private void Awake()
    {
        BoundsInt bounds = tilemap.cellBounds;
        Debug.Log(tilemap.GetTilesBlock(bounds).Length); // 혹은 내 캐릭 주변으로 감지
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }
}
