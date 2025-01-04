using A_Pathfinding.Pathfinding;
using SelectionSystem;
using SelectionSystem.Marker;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour, ISelectable
{
    public UnityEvent onSelect;
    public UnityEvent onDeselect;

    private PathfindingAgent _agent;
    private SelectionMarker _marker;
    private void Awake()
    {
        _agent = GetComponent<PathfindingAgent>();
    }

    #region Selection System
    public void Select(SelectionMarker selectionMarker)
    {
        Debug.Log($"{name} selected");

        onSelect.Invoke();

        _marker = selectionMarker;
        _marker.AttachTo(transform, Vector3.zero, transform.localScale);
    }

    public void Deselect()
    {
        Debug.Log($"{name} deselected");

        onDeselect.Invoke();

        if (_marker) _marker.Detach();
    }

    public void Execute(Vector3 position)
    {
        _agent.MoveToPosition(position);
    }
    #endregion
}
