using UnityEngine;

namespace TileSystem
{
    public class GridManager : MonoBehaviour
    {

    
        public Vector2Int gridWorldSize;
        public float cellSize;

        private float cellHalfSize;
        private Vector3 worldBottomLeft;
        private Cell[,] cells;
    
        public void Init(Vector2Int gridWorldSize, float cellSize)
        {
            this.gridWorldSize = gridWorldSize;
            this.cellSize = cellSize;
            cellHalfSize = cellSize / 2;

            worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
           
            CreateGrid();
        }
        void CreateGrid()
        {
            cells = new Cell[gridWorldSize.x, gridWorldSize.y];

            for (int x = 0; x < gridWorldSize.x; x++)
            {
                for (int y = 0; y < gridWorldSize.y; y++)
                {
                    Vector3 start = worldBottomLeft + new Vector3(x * cellSize, y * cellSize);
                    Vector3 endX = start + new Vector3(0, gridWorldSize.y * cellSize);
                    Vector3 endY = start + new Vector3(gridWorldSize.x * cellSize, 0);

                    Debug.DrawLine(start, endX, Color.white, 100f);
                    Debug.DrawLine(start, endY, Color.white, 100f);

                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * cellSize + cellHalfSize) + Vector3.up * (y * cellSize + cellHalfSize);
                    cells[x, y] = new Cell(worldPoint, x, y);
                }
            }
        }
        public Cell GetCellFromWorlPosition(Vector3 worldPosition)
        {
            var pos = GetGridIndexPosition(worldPosition);
            return cells[pos.x, pos.y];
        }
        public Vector2Int GetGridIndexPosition(Vector3 worldPosition)
        {
            int x = Mathf.FloorToInt((worldPosition.x - worldBottomLeft.x) / cellSize);
            int y = Mathf.FloorToInt((worldPosition.y - worldBottomLeft.y) / cellSize);
            return new Vector2Int(x, y);
        }
        public Vector3 GetSnapPosition(Vector3 worldPosition)
        {
            var indexPosition = GetGridIndexPosition(worldPosition);
            return cells[indexPosition.x, indexPosition.y].worldPosition;
        }
        public Vector3 GetWorldPosition(Vector2Int gridPosition)
        {
            float x = gridPosition.x * cellSize + worldBottomLeft.x;
            float y = gridPosition.y * cellSize + worldBottomLeft.y;
            return new Vector3(x, y);
        }
    }

}
