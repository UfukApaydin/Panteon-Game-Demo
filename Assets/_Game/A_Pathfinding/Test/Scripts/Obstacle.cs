using A_Pathfinding.Nodes;
using UnityEngine;
using PathfindingGrid = A_Pathfinding.Nodes.PathfindingGrid;

namespace A_Pathfinding.Test
{
    public class Obstacle : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            PathfindingGrid grid = FindAnyObjectByType<PathfindingGrid>();
            grid.UpdateNodesWalkable(transform.position, 2, false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}