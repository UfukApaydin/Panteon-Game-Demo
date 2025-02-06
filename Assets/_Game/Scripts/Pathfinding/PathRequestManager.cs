using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace AStarPathfinding
{
    public class PathRequestManager : MonoBehaviour
    {
  
        Queue<PathResult> results = new Queue<PathResult>();

        static PathRequestManager instance;
        private Pathfinding _pathfinding;
        private PathfindingGrid _grid;
        void Awake()
        {
            if (instance == null)
                instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        void Update()
        {
            if (results.Count > 0)
            {
                int itemsInQueue = results.Count;
                lock (results)
                {
                    for (int i = 0; i < itemsInQueue; i++)
                    {
                        PathResult result = results.Dequeue();
                        result.callback(result.path, result.success);
                    }
                }
            }
        }

        public static void RequestPath(PathRequest request)
        {
            //ThreadStart threadStart = delegate
            //{
            //    instance._pathfinding.FindPath(request, instance.FinishedProcessingPath);
            //};
            //threadStart.Invoke();
            Thread thread = new Thread(() =>
            {
                instance._pathfinding.FindPath(request, instance.FinishedProcessingPath);
            });
            thread.Start();
        }

        public void FinishedProcessingPath(PathResult result)
        {
            lock (results)
            {
                results.Enqueue(result);
            }
        }
        public static void ReleaseNode(PathfindingAgent agent)
        {
            instance._grid.NodeFromWorldPoint(agent.transform.position).ReleaseNode(agent);
        }


        #region Builder
        public class Builder
        {   
            public PathRequestManager Build(Pathfinding pathfinding, PathfindingGrid grid)
            {
                PathRequestManager manager = new GameObject("PathRequestManager").AddComponent<PathRequestManager>();
                manager._pathfinding = pathfinding;
                manager._grid = grid;
                return manager;
            }
        }

        #endregion
    }
    public struct PathResult
    {
        public Vector3[] path;
        public bool success;
        public Action<Vector3[], bool> callback;

        public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
        {
            this.path = path;
            this.success = success;
            this.callback = callback;
        }

    }

    public struct PathRequest
    {
        public PathfindingAgent requestedAgent;
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(PathfindingAgent requestedAgent,Vector3 start, Vector3 end, Action<Vector3[], bool> callback)
        {
            this.requestedAgent = requestedAgent;
            pathStart = start;
            pathEnd = end;
            this.callback = callback;
        }

    }


}
