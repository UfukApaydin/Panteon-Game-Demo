using ObjectPoolSystem;
using UnityEngine;

public class Waypoint
{
    private Vector3 _parentPosition;
    private Vector3 _position;
    private readonly PoolSystem _waypointPool;
    private IPoolable _waypointObj;
  
    public Waypoint(PoolSystem waypointPool, Vector3 parentPosition, Vector3 position)
    {
        _waypointPool = waypointPool;
        _parentPosition = parentPosition;
        _position = position;

        Init();
    }
    private void Init()
    {
        _waypointPool.Init();
    }
    public void ActivateWaypoint()
    {
        if ( _waypointObj != null)
            return;
        _waypointObj = _waypointPool.Get();
        MoveWaypoint(_position);
    }
    public void DeactivateWaypoint()
    {
        if (_waypointObj != null)
        {
            _waypointPool.Return(_waypointObj);
            _waypointObj = null;
        }
       
    }
    public void MoveWaypoint(Vector3 position)
    {
        if (_waypointObj == null)
            return;
        _position = position;
        _waypointObj.GameObject.transform.position = position;
        _waypointObj.UpdateArgs(_parentPosition);
    }
    public Vector3 GetWaypointPosition()
    {
        return _position;
    }
}