using UnityEngine;

namespace GridSystem
{
    public class GridManager
    {
        private GridConfig _gridConfig;
        private Cell[,] _cells;

        public GridManager(GridConfig gridConfig)
        {
            _gridConfig = gridConfig;
            CreateGrid();
        }
        public void CreateGrid()
        {
            _cells = new Cell[_gridConfig.gridWorldSize.x, _gridConfig.gridWorldSize.y];

            for (int x = 0; x < _gridConfig.gridWorldSize.x; x++)
            {
                for (int y = 0; y < _gridConfig.gridWorldSize.y; y++)
                {
                    if (_gridConfig.drawLines)
                    {
                        Vector3 start = _gridConfig.WorldBottomLeft + new Vector3(x * _gridConfig.cellSize, y * _gridConfig.cellSize);
                        Vector3 endX = start + new Vector3(0, _gridConfig.gridWorldSize.y * _gridConfig.cellSize);
                        Vector3 endY = start + new Vector3(_gridConfig.gridWorldSize.x * _gridConfig.cellSize, 0);

                        Debug.DrawLine(start, endX, Color.white, 100f);
                        Debug.DrawLine(start, endY, Color.white, 100f);

                    }
                    Vector3 worldPoint = _gridConfig.WorldBottomLeft + Vector3.right * (x * _gridConfig.cellSize + _gridConfig.CellHalfSize) + Vector3.up * (y * _gridConfig.cellSize + _gridConfig.CellHalfSize);
                    _cells[x, y] = new Cell(worldPoint, x, y);
                }
            }
        }
        public Cell GetCellFromWorlPosition(Vector3 worldPosition)
        {
            var pos = GetGridIndexPosition(worldPosition);
            return _cells[pos.x, pos.y];
        }
        public Cell GetCellFromIndex(Vector2Int index)
        {
            return _cells[index.x, index.y];
        }
        public Vector2Int GetGridIndexPosition(Vector3 worldPosition)
        {
            int x = Mathf.FloorToInt((worldPosition.x - _gridConfig.WorldBottomLeft.x) / _gridConfig.cellSize);
            int y = Mathf.FloorToInt((worldPosition.y - _gridConfig.WorldBottomLeft.y) / _gridConfig.cellSize);
            return new Vector2Int(x, y);
        }
        public Vector3 GetSnapPosition(Vector3 worldPosition)
        {
            var indexPosition = GetGridIndexPosition(worldPosition);
            return _cells[indexPosition.x, indexPosition.y].worldPosition;
        }
    }

}
