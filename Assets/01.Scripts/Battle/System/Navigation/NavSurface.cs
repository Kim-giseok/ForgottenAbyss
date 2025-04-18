using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NavSurface : MonoBehaviour
{
    public Vector2Int area;
    public List<Cell> cells = new();
    public LayerMask layerMask;
    
    public Tilemap tilemap;

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
    
    private void Awake()
    {
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
            var hit = Physics2D.OverlapBox(point, new Vector2(0.9f, 0.9f), 0f, layerMask);
            
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
            if (left != null && left.platformID != -1) { cell.platformID = left.platformID; }
            else { cell.platformID = currentPlatformID++; }
        }
    }
    
    Color PlatformColor(int id)
    {
        Random.InitState(id * 1000);
        return new Color(Random.value, Random.value, Random.value);
    }
    
    void OnDrawGizmosSelected()
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