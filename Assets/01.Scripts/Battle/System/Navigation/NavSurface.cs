using System.Collections.Generic;
using UnityEngine;

public class NavSurface : MonoBehaviour
{
    public Vector2Int area;
    // private float cellSize = 1f;
    
    private List<(Vector2 position, bool isWall)> cells = new();
    private List<(Vector2 position, bool isWall)> upperCells = new();
    public LayerMask layerMask;

    void ScanArea()
    {
        for (int coordY = -(area.y / 2) ; coordY < (area.y / 2); coordY++)
        {
            for (int coordX = -(area.x / 2); coordX < (area.x / 2); coordX++)
            {
                var point = new Vector2(transform.position.x + coordX + 0.5f, transform.position.y + coordY + 0.5f);
                
                var hit = Physics2D.OverlapBox(point, new Vector2(0.9f, 0.9f), 0f, layerMask); 
                if (hit)
                {
                    var upper = cells.Find(upper => Mathf.Approximately(upper.position.x, point.x) && Mathf.Approximately(upper.position.y, point.y + 1));
                    if (upper.isWall)
                    {
                        cells.Add((point, false));
                    } 
                    else {
                        cells.Add((point, true));
                    }
                }
                else
                {
                    cells.Add((point, false));
                }
            }
        }
        
        
        foreach (var current in cells)
        {
            var upper = cells.Find(cell => 
                Mathf.Approximately(cell.position.x, current.position.x) && 
                Mathf.Approximately(cell.position.y, current.position.y + 1));

            if (upper.isWall && current.isWall)
            {
                upperCells.Add((current.position, false));
            }
            else
            {
                // upperCells.Add(current);
            }
        }
        
        foreach (var curr in upperCells) {}
    }

    private void Awake()
    {
        cells.Clear();
        ScanArea();
    }
    
    void OnDrawGizmos()
    {
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(area.x, area.y, 1));

        foreach (var cell in cells)
        {
            // Gizmos.color = cell.isWall ? Color.red : Color.green;
            // Gizmos.DrawWireCube(cell.position, Vector2.one);

            if (cell.isWall)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(cell.position, 0.1f);
            }
        }
        
        foreach (var cell in upperCells)
        {
            if (cell.isWall)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(cell.position, 0.1f);
            }
        }
    }
}