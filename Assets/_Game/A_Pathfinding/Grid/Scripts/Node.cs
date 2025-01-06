using UnityEngine;

namespace AStarPathfinding
{
    public class Node :IHeapItem<Node>
    {
        public bool walkable;
        public bool occupied;
        public PathfindingAgent occupiedAgent;
        public Vector3 worldPosition;
        public int gridX;
        public int gridY;
    

        public int gCost;
        public int hCost;
        public Node parent;
        private int _heapIndex;

        public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
        {
            this.walkable = walkable;
            this.worldPosition = worldPosition;
            this.gridX = gridX;
            this.gridY = gridY;
        
        }

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }

        public int HeapIndex
        {
            get
            {
                return _heapIndex;
            }
            set
            {
                _heapIndex = value;
            }
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }
            return -compare;
        }

        public bool IsOccupied()
        {
            return occupiedAgent != null || !walkable;
        }

        private static readonly object occupancyLock = new object();

        public bool TryOccupyNode(PathfindingAgent agent)
        {
            lock (occupancyLock)
            {
                if (occupiedAgent == null || occupiedAgent == agent)
                {
                    occupiedAgent = agent;
                    return true;
                }
                return false;
            }
        }

        public void ReleaseNode(PathfindingAgent agent)
        {
            lock (occupancyLock)
            {
                if (occupiedAgent == agent)
                {
                    occupiedAgent = null;
                }
            }
        }
    }

}
