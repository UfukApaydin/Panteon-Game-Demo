using A_Pathfinding.Nodes;
using System.Collections;
using System.IO;
using UnityEngine;
using Grid = A_Pathfinding.Nodes.PathfindingGrid;
namespace A_Pathfinding.Pathfinding
{
    public class Unit : MonoBehaviour
    {
        const float minPathUpdateTime = .2f;
        const float pathUpdateMoveThreshold = .5f;

        public Vector3 target;
        public float speed = 20;
        public float turnSpeed = 3;
        public float turnDst = 5;
        public float stoppingDst = 10;

        private int targetIndex;
       // Path path;
        Vector3[] path;
        void Start()
        {
          //  StartCoroutine(UpdatePath());
        }

        public void MoveToPosition(Vector3 targetPosition)
        {
            target = targetPosition;
            UpdatePath();
        }

        public void UpdatePath()
        {
            PathRequestManager.RequestPath( new PathRequest(transform.position, target, OnPathFound));
        }
        public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
        {
            if (pathSuccessful)
            {
                path = newPath;
                targetIndex = 0;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        IEnumerator FollowPath()
        {
            Vector3 currentWaypoint = path[0];

            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;

            }
        }
     
        public void OnDrawGizmos()
        {
            if (path != null)
            {
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i], Vector3.one);

                    if (i == targetIndex)
                    {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }
        //public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
        //{
        //    if (pathSuccessful)
        //    {
        //        path = new Path(waypoints, transform.position, turnDst, stoppingDst);

        //        StopCoroutine("FollowPath");
        //        StartCoroutine("FollowPath");
        //    }
        //}

        //IEnumerator UpdatePath()
        //{

        //    if (Time.timeSinceLevelLoad < .3f)
        //    {
        //        yield return new WaitForSeconds(.3f);
        //    }
        //    PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound));

        //    float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        //    Vector3 targetPosOld = target;

        //    while (true)
        //    {
        //        yield return new WaitForSeconds(minPathUpdateTime);
        //        print(((target - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
        //        if ((target - targetPosOld).sqrMagnitude > sqrMoveThreshold)
        //        {
        //            PathRequestManager.RequestPath(new PathRequest(transform.position, target, OnPathFound));
        //            targetPosOld = target;
        //        }
        //    }
        //}

        //IEnumerator FollowPath()
        //{

        //    bool followingPath = true;
        //    int pathIndex = 0;
        //    transform.LookAt(path.lookPoints[0]);

        //    float speedPercent = 1;

        //    while (followingPath)
        //    {
        //        Vector2 pos2D = new Vector2(transform.position.x, transform.position.y);
        //        while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
        //        {
        //            if (pathIndex == path.finishLineIndex)
        //            {
        //                followingPath = false;
        //                break;
        //            }
        //            else
        //            {
        //                pathIndex++;
        //            }
        //        }

        //        if (followingPath)
        //        {

        //            if (pathIndex >= path.slowDownIndex && stoppingDst > 0)
        //            {
        //                speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
        //                if (speedPercent < 0.01f)
        //                {
        //                    followingPath = false;
        //                }
        //            }

        //            //Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
        //            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        //            //transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);

        //            Vector3 nextWaypoint = path.lookPoints[pathIndex];
        //            Vector2 direction2D = new Vector2(nextWaypoint.x - transform.position.x,
        //                                              nextWaypoint.y - transform.position.y).normalized;
        //            float angle = Mathf.Atan2(direction2D.y, direction2D.x) * Mathf.Rad2Deg - 90f;

        //            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        //            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);

        //            // 4) Move forward (local up in 2D)
        //            transform.Translate(Vector3.up * speed * speedPercent * Time.deltaTime, Space.Self);
        //        }

        //        yield return null;

        //    }
        //}

        //public void OnDrawGizmos()
        //{
        //    if (path != null)
        //    {
        //        path.DrawWithGizmos();
        //    }
        //}
    }
}