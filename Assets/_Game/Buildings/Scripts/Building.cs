using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingData config;
  
    private List<Vector2Int> cellPositions;

    public void Init(BuildingData config)
    {
        this.config = config;
    }
    public void Build(Cell cell)
    {
        transform.position = cell.worldPosition + config.CenterOffset;

        cellPositions = new(); 
        for (int x = 0; x < config.size.x; x++)
        {
            for (int y = 0; y < config.size.y; y++)
            {
                cellPositions.Add(new Vector2Int(cell.gridX + x, cell.gridY + y));   
            }
        }
        GameInitiator.Instance.pathfindingDirector.grid.UpdateNodesWalkableCell(cellPositions.ToArray(), false);
      
    }

}
