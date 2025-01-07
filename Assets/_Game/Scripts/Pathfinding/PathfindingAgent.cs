using System.Collections;
using UnityEngine;

namespace AStarPathfinding
{
    public class PathfindingAgent : MonoBehaviour
    {
        const float minPathUpdateTime = .2f;
        const float pathUpdateMoveThreshold = .5f;

        public Vector3 target;
        public float speed = 20;
        public float turnSpeed = 50;

        private int _targetIndex;
        private Vector3[] _path;
        private bool _isMoving = false;
        private void OnEnable()
        {
            _isMoving = false;   
        }
        public void MoveToPosition(Vector3 targetPosition)
        {
            target = targetPosition;
            UpdatePath();
        }

        public void UpdatePath()
        {
            PathRequestManager.RequestPath(new PathRequest(this, transform.position, target, OnPathFound));
        }
        public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
        {
            if (pathSuccessful)
            {
                _path = newPath;
                _targetIndex = 0;
                StopCoroutine(nameof(FollowPath));
                StartCoroutine(nameof(FollowPath));
            }
        }
        IEnumerator FollowPath()
        {
            Vector3 currentWaypoint = _path[0];

            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    _targetIndex++;
                    if (_targetIndex >= _path.Length)
                    {
                        _isMoving = false;
                        yield break;
                    }
                    currentWaypoint = _path[_targetIndex];
                }


                LerpRotateToTarget(currentWaypoint);

                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                _isMoving = true;
                yield return null;

            }
        }

        public bool IsMoving { get { return _isMoving; } }
        public void LerpRotateToTarget(Vector3 targetPositon)
        {
            Vector3 direction = (targetPositon - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation,Time.deltaTime * turnSpeed);
        }
        public void SetRotationToTarget(Vector3 targetPositon)
        {
            Vector3 direction = (targetPositon - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        public void OnDrawGizmos()
        {
            if (_path != null)
            {
                for (int i = _targetIndex; i < _path.Length; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(_path[i], Vector3.one);

                    if (i == _targetIndex)
                    {
                        Gizmos.DrawLine(transform.position, _path[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(_path[i - 1], _path[i]);
                    }
                }
            }
        }
        /// <summary>
        /// Removes the occupancy from the node. Use this on destroying the object
        /// </summary>
        public void ResetOccupyNode()
        {
            PathRequestManager.ReleaseNode(this);
        }

        //IEnumerator UpdatePath()
        //{

        //    if (Time.timeSinceLevelLoad < .3f)
        //    {
        //        yield return new WaitForSeconds(.3f);
        //    }
        //    PathRequestManager.RequestPath(new PathRequest(transform.position, _target, OnPathFound));

        //    float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        //    Vector3 targetPosOld = _target;

        //    while (true)
        //    {
        //        yield return new WaitForSeconds(minPathUpdateTime);
        //        print(((_target - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
        //        if ((_target - targetPosOld).sqrMagnitude > sqrMoveThreshold)
        //        {
        //            PathRequestManager.RequestPath(new PathRequest(transform.position, _target, OnPathFound));
        //            targetPosOld = _target;
        //        }
        //    }
        //}



    }
}