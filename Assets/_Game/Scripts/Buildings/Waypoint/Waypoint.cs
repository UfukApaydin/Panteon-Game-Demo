using ObjectPoolSystem;
using UnityEngine;

public class Waypoint
{
    private Vector3 _parentPosition;
    private Vector3 _position;
    private PoolManager poolManager => ServiceLocator.Get<PoolManager>();
    private IPoolable _waypointObj;
  
    public Waypoint(Vector3 parentPosition, Vector3 position)
    {

        _parentPosition = parentPosition;
        _position = position;


    }
    public void ActivateWaypoint()
    {
        if ( _waypointObj != null)
            return;
        _waypointObj = poolManager.GetObject(GameManager.Instance.gameData.waypointType);
        MoveWaypoint(_position);
    }
    public void DeactivateWaypoint()
    {
        if (_waypointObj != null)
        {
            poolManager.ReturnObject(GameManager.Instance.gameData.waypointType, _waypointObj);
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