using System.Collections.Generic;
using UnityEngine;

namespace A_Pathfinding.Nodes
{
    public class PathfindingGrid 
    {
        public bool displayGridGizmos = true;

        public readonly float nodeRadius;
        private Vector3 originPosition;
        //private LayerMask unwalkableMask;
        private Vector2 gridWorldSize;
        private Node[,] grid;
        private float nodeDiameter;
        private int gridSizeX, gridSizeY;


        public int MaxSize => gridSizeX * gridSizeY;

         PathfindingGrid(Vector3 originPosition, Vector2 gridWorldSize, float nodeRadius/*, LayerMask unwalkableMask*/)
        {
            this.originPosition = originPosition;
            this.gridWorldSize = gridWorldSize;
            this.nodeRadius = nodeRadius;
          //  this.unwalkableMask = unwalkableMask;

            nodeDiameter = nodeRadius * 2;
        }
        public void StartGrid()
        {
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            CreateGrid();
        }
        void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY];
            Vector3 worldBottomLeft = originPosition - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                    bool walkable = true;
                    //   bool walkable  = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));


                    grid[x, y] = new Node(walkable, worldPoint, x, y/*, movementPenalty*/);
                }
            }
        }
        public void UpdateNodesWalkableCell(Vector2Int[] cellIndexes, bool isWalkable)
        {
            foreach (Vector2Int cellIndex in cellIndexes)
            {
                grid[cellIndex.x, cellIndex.y].walkable = isWalkable;
                Debug.Log($"Cell: {cellIndex.x} : {cellIndex.y}: changed");          
            }
        }
        public List<Node> GetNeighbours(Node node, int radius = 1)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

        public Node FindClosestWalkableNode(Node startNode)
        {
            // If the node is already walkable, just return it
            if (startNode.walkable)
            {
                return startNode;
            }

            // Typical BFS data structures
            Queue<Node> queue = new Queue<Node>();
            HashSet<Node> visited = new HashSet<Node>();

            // Begin from the 'startNode' (which is unwalkable),
            // and search outward until we find a walkable node.
            queue.Enqueue(startNode);
            visited.Add(startNode);

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();

                // Check neighbors
                foreach (Node neighbor in GetNeighbours(current))
                {
                    if (!visited.Contains(neighbor))
                    {
                        // If neighbor is walkable, return immediately
                        if (neighbor.walkable)
                        {
                            return neighbor;
                        }
                        // Otherwise keep searching
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }

            // If we exhaust the BFS and find no walkable node, return null
            return null;
        }

        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
            return grid[x, y];
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(originPosition, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
            if (grid != null && displayGridGizmos)
            {
                foreach (Node n in grid)
                {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
                }
            }
        }

        [System.Serializable]
        public class TerrainType
        {
            public LayerMask terrainMask;
            public int terrainPenalty;
        }

        public void ChangeNodeWalkable(Vector3 position, bool walkable)
        {
            NodeFromWorldPoint(position).walkable = walkable;
        }

        #region Builder
        public class Builder
        {
            private Vector3 originPosition = Vector3.zero;
            private Vector2 gridWorldSize = new Vector2(50,50); 
            private float nodeRadius = 1;
            //private LayerMask unwalkableMask;

            public Builder SetOriginPosition(Vector3 position)
            { 
                originPosition = position;
                return this;
            }
            public Builder SetGridWorldSize(Vector2 size)
            {
                gridWorldSize = size;
                return this;
            }
            public Builder SetNodeRadius(float radius)
            {
                nodeRadius = radius;
                return this;
            }
            //public Builder SetLayerMask(LayerMask layerMask)
            //{
            //    unwalkableMask = layerMask;
            //    return this;
            //}
            public PathfindingGrid Build()
            {
                return new PathfindingGrid(originPosition, gridWorldSize, nodeRadius/*, unwalkableMask*/);
            }
            public PathfindingGrid BuildAndStart()
            {
                PathfindingGrid pathfindingGrid = Build();
                pathfindingGrid.StartGrid();
                return pathfindingGrid;
            }
                
        }

        #endregion
    }
}
