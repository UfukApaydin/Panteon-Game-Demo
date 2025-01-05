using UnityEngine;

namespace GridSystem
{
    public class Cell
    {
        public Vector3 worldPosition;
        public int gridX;
        public int gridY;

        public Cell(Vector3 worldPosition, int gridX, int gridY)
        {
            this.worldPosition = worldPosition;
            this.gridX = gridX;
            this.gridY = gridY;
        }
    }
}