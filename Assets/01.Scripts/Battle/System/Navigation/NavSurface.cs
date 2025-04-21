using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// 셀 포지션이 곧 world 포지션
public class Cell
{
    public Vector2 WorldPos;
    public Vector3Int tilePos;
        
    public TileBase tile;
        
    public bool isWall;
    public int platformID = -1;

    public Cell(Vector2 worldPos, Vector3Int tilePos, TileBase tile, bool isWall)
    {
        this.WorldPos = worldPos;
        this.tilePos = tilePos;
            
        this.tile = tile;
        this.isWall = isWall;
    }
}

public class Platform
{
    public int id = -1;
    public List<Cell> cells = new();
    public Cell centerCell => cells[cells.Count / 2];
}

public class NavSurface : MonoBehaviour
{
    public Dictionary<GameObject, int> targetPlatforms { get; private set; } = new();
    public static NavSurface Instance;
    
    public Vector2Int area; // 타일맵 자체 너비
    public List<Cell> cells = new();
    public List<Platform> platforms = new();
    
    public LayerMask layerMask { get; private set; }
    public Tilemap tilemap { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        if(!Instance) { Instance = FindObjectOfType<NavSurface>(); }
    }
    
    private void Awake()
    {
        layerMask = gameObject.layer;
        tilemap = gameObject.GetComponent<Tilemap>();
        
        cells.Clear();
        ScanArea();
    }

    void ScanArea()
    {
        // 그냥 wall point에만 그리는 것은 어떨까?
        for (int coordY = -(area.y / 2) ; coordY < (area.y / 2); coordY++)
        for (int coordX = -(area.x / 2); coordX < (area.x / 2); coordX++)
        {
            // 0,0 이니 하단부터 시작
            var point = new Vector2(transform.position.x + coordX + 0.5f, transform.position.y + coordY + 0.5f);
            var hit = Physics2D.OverlapBox(point, new Vector2(0.9f, 0.9f), 0f, 1 << layerMask);
            
            Debug.Log(hit.ToString());
            
            if (hit)
            {
                var downWall = cells.Find(cell => Mathf.Approximately(cell.WorldPos.x, point.x) && Mathf.Approximately(cell.WorldPos.y, point.y - 1));
                if (downWall is { isWall: true }) { downWall.isWall = false; }

                var cellPos = tilemap.WorldToCell(point);
                TileBase tile = tilemap.GetTile(cellPos);
                
                cells.Add(new Cell(point, cellPos, tile, true));
            }
        }


        var topCells = cells.FindAll(c => c.isWall);
        int currentPlatformID = 0;

        foreach (var cell in topCells)
        {
            // 왼쪽에 바로 붙은 cell 찾기 (x - 1, y 같은 위치)
            var left = topCells.Find(c => Mathf.Approximately(c.WorldPos.x, cell.WorldPos.x - 1f) && Mathf.Approximately(c.WorldPos.y, cell.WorldPos.y));
            if (left != null && left.platformID != -1)
            {
                // notice: 비용적 효율을 위해 어떻게 개선해야하는 지 생가해보기
                cell.platformID = left.platformID;
                platforms.Find(platform => platform.id == left.platformID).cells.Add(cell);
            }
            else
            {
                cell.platformID = ++currentPlatformID;
                Platform newPlatform = new () { id = cell.platformID };
                newPlatform.cells.Add(cell);
                platforms.Add(newPlatform);
            }
        }
    }
    
    Color PlatformColor(int id)
    {
        Random.InitState(id * 1000);
        return new Color(Random.value, Random.value, Random.value);
    }
    
    // 계속 실행되는 점 확인 필요
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(area.x, area.y, 1));

        foreach (var cell in cells)
        {
            if (cell.isWall)
            {
                Gizmos.color = PlatformColor(cell.platformID); // 플랫폼 별 색상
                Gizmos.DrawSphere(cell.WorldPos, 0.15f);
            }
        }
    }
}