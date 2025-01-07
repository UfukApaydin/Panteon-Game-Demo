using UnityEngine;

namespace GridSystem
{
    [CreateAssetMenu(fileName = "New_GridConfig", menuName = "Scriptable Objects/Configs/GridConfig")]
    public class GridConfig : ScriptableObject
    { 
        public Vector3 originPosition;
        public Vector2Int gridWorldSize;
        public float cellSize;
        public bool drawLines = false;

        public float CellHalfSize => cellSize * 0.5f;
        public Vector3 WorldBottomLeft => originPosition - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
       
    }
}