using System;
using System.Collections.Generic;
using UnityEngine;


namespace AStarPathfinding
{
    public class Pathfinding
    {
        private PathfindingGrid _grid;

        public Pathfinding(PathfindingGrid grid)
        {
            _grid = grid;
        }
        public void FindPath(PathRequest request, Action<PathResult> callback)
        {

            Vector3[] waypoints = new Vector3[0];
            bool pathSuccess = false;

            Node startNode = _grid.NodeFromWorldPoint(request.pathStart);
            Node targetNode = _grid.NodeFromWorldPoint(request.pathEnd);
            startNode.parent = startNode;


            if (targetNode.IsOccupied() && targetNode.occupiedAgent != request.requestedAgent)
            {
                
                Node closestWalkable = _grid.FindClosestWalkableNode(targetNode);

                if (closestWalkable != null)
                {
                    targetNode = closestWalkable;

                }
                else
                {
                    // If no walkable node found, _path fails immediately
                    // callback with failure or return
                    callback(new PathResult(new Vector3[0], false, request.callback));
                    return;
                }
            }

            if (startNode.walkable && targetNode.walkable)
            {
                Heap<Node> openSet = new Heap<Node>(_grid.MaxSize);
                HashSet<Node> closedSet = new HashSet<Node>();
                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    Node currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        //      sw.Stop();
                        //print ("Path found: " + sw.ElapsedMilliseconds + " ms");
                        pathSuccess = true;
                        break;
                    }

                    foreach (Node neighbour in _grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.walkable || closedSet.Contains(neighbour))
                        {
                            continue;
                        }

                        int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) /*+ neighbour.movementPenalty*/;
                        if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, targetNode);
                            neighbour.parent = currentNode;

                            if (!openSet.Contains(neighbour))
                                openSet.Add(neighbour);
                            else
                                openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
            if (pathSuccess)
            {
                startNode.ReleaseNode(request.requestedAgent);
                targetNode.TryOccupyNode(request.requestedAgent);
                waypoints = RetracePath(startNode, targetNode);
                pathSuccess = waypoints.Length > 0;
            }
            callback(new PathResult(waypoints, pathSuccess, request.callback));

        }


        Vector3[] RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);
            return waypoints;

        }

        Vector3[] SimplifyPath(List<Node> path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;

            if (path.Count == 1) // On 1 cell movement this ensures the _path still exist.
            {
                waypoints.Add(path[0].worldPosition);
            }

            for (int i = 1; i < path.Count; i++)
            {

                Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
                if (directionNew != directionOld)
                {
                    waypoints.Add(path[i - 1].worldPosition);
                }
                directionOld = directionNew;
            }
            return waypoints.ToArray();
        }

        int GetDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
}