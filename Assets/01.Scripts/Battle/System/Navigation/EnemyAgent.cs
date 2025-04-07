using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

// 좌우 값을 기준으로 이동 가능한지 아닌지 우선 체크
public class EnemyAgent : MonoBehaviour
{
    public GameObject player { get; private set; }
    
    public float detectedDistance;
    public float stoppingDistance;
    public float tracingSpeed;
    
    public Tilemap tilemap; // 추후 동적으로 찾도록 처리

    private void Awake()
    {
        BoundsInt bounds = tilemap.cellBounds;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public float GetDistance()
    {
        return (transform.position - player.transform.position).magnitude;
    }
}
