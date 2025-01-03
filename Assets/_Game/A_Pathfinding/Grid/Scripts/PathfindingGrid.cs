using System.Collections.Generic;
using UnityEngine;

namespace A_Pathfinding.Nodes
{
    public class PathfindingGrid : MonoBehaviour
    {
        public bool displayGridGizmos;
        public LayerMask unwalkableMask;
        public Vector2 gridWorldSize;
        public float nodeRadius;
        //public TerrainType[] walkableRegions;
        //public int obstacleProximityPenalty = 10;
        //Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();
        //LayerMask walkableMask;

        Node[,] grid;

        float nodeDiameter;
        int gridSizeX, gridSizeY;

        //int penaltyMin = int.MaxValue;
        //int penaltyMax = int.MinValue;
        public int MaxSize => gridSizeX * gridSizeY;

        // void Awake()
        // {


        //     //foreach (TerrainType region in walkableRegions)
        //     //{
        //     //    walkableMask.value |= region.terrainMask.value;
        //     //    walkableRegionsDictionary.Add((int)Mathf.Log(region.terrainMask.value, 2), region.terrainPenalty);
        //     //}

        ////     CreateGrid();
        // }
        public void Init(Vector2 gridWorldSize, float nodeRadius)
        {
            this.gridWorldSize = gridWorldSize;
            this.nodeRadius = nodeRadius;

            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            CreateGrid();
        }
        void CreateGrid()
        {
            grid = new Node[gridSizeX, gridSizeY];
            Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));

                    //int movementPenalty = 0;


                    //Ray ray = new Ray(worldPoint + Vector3.up * 50, Vector3.down);
                    //RaycastHit hit;
                    //if (Physics.Raycast(ray, out hit, 100, walkableMask))
                    //{
                    //    walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                    //}

                    //if (!walkable)
                    //{
                    //    movementPenalty += obstacleProximityPenalty;
                    //}


                    grid[x, y] = new Node(walkable, worldPoint, x, y/*, movementPenalty*/);
                }
            }

            //   BlurPenaltyMap(3);

        }

        //void BlurPenaltyMap(int blurSize)
        //{
        //    int kernelSize = blurSize * 2 + 1;
        //    int kernelExtents = (kernelSize - 1) / 2;

        //    int[,] penaltiesHorizontalPass = new int[gridSizeX, gridSizeY];
        //    int[,] penaltiesVerticalPass = new int[gridSizeX, gridSizeY];

        //    for (int y = 0; y < gridSizeY; y++)
        //    {
        //        for (int x = -kernelExtents; x <= kernelExtents; x++)
        //        {
        //            int sampleX = Mathf.Clamp(x, 0, kernelExtents);
        //            penaltiesHorizontalPass[0, y] += grid[sampleX, y].movementPenalty;
        //        }

        //        for (int x = 1; x < gridSizeX; x++)
        //        {
        //            int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, gridSizeX);
        //            int addIndex = Mathf.Clamp(x + kernelExtents, 0, gridSizeX - 1);

        //            penaltiesHorizontalPass[x, y] = penaltiesHorizontalPass[x - 1, y] - grid[removeIndex, y].movementPenalty + grid[addIndex, y].movementPenalty;
        //        }
        //    }

        //    for (int x = 0; x < gridSizeX; x++)
        //    {
        //        for (int y = -kernelExtents; y <= kernelExtents; y++)
        //        {
        //            int sampleY = Mathf.Clamp(y, 0, kernelExtents);
        //            penaltiesVerticalPass[x, 0] += penaltiesHorizontalPass[x, sampleY];
        //        }

        //        int blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, 0] / (kernelSize * kernelSize));
        //        grid[x, 0].movementPenalty = blurredPenalty;

        //        for (int y = 1; y < gridSizeY; y++)
        //        {
        //            int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, gridSizeY);
        //            int addIndex = Mathf.Clamp(y + kernelExtents, 0, gridSizeY - 1);

        //            penaltiesVerticalPass[x, y] = penaltiesVerticalPass[x, y - 1] - penaltiesHorizontalPass[x, removeIndex] + penaltiesHorizontalPass[x, addIndex];
        //            blurredPenalty = Mathf.RoundToInt((float)penaltiesVerticalPass[x, y] / (kernelSize * kernelSize));
        //            grid[x, y].movementPenalty = blurredPenalty;

        //            if (blurredPenalty > penaltyMax)
        //            {
        //                penaltyMax = blurredPenalty;
        //            }
        //            if (blurredPenalty < penaltyMin)
        //            {
        //                penaltyMin = blurredPenalty;
        //            }
        //        }
        //    }

        //}

        public void UpdateNodesWalkable(Vector3 originPosition, int radius, bool isWalkable)
        {
            Node originNode = NodeFromWorldPoint(originPosition);
            List<Node> nodes = GetNeighbours(originNode, radius);
            nodes.Add(originNode);
            foreach (Node node in nodes)
            {
                node.walkable = !(Physics2D.OverlapCircle(node.worldPosition, nodeRadius, unwalkableMask));

            }
        }
        public void UpdateNodesWalkableCell(Vector2Int[] cellIndexes, bool isWalkable)
        {
            foreach (Vector2Int cellIndex in cellIndexes)
            {
                grid[cellIndex.x, cellIndex.y].walkable = isWalkable;
                Debug.Log($"Cell: {cellIndex.x} : {cellIndex.y}: changed");
                //  node.walkable = isWalkable;
            }
            // Node originNode = NodeFromWorldPoint(originPosition);

            //nodes.Add(originNode);
            //foreach (Node node in nodes)
            //{
            //    node.walkable = !(Physics2D.OverlapCircle(node.worldPosition, nodeRadius, unwalkableMask));

            //}
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
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
            if (grid != null && displayGridGizmos)
            {
                foreach (Node n in grid)
                {

                    //   Gizmos.color = Color.Lerp(Color.white, Color.black, Mathf.InverseLerp(penaltyMin, penaltyMax,n.movementPenalty));
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

    }
}
