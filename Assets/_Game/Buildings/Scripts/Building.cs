using SelectionSystem;
using SelectionSystem.Marker;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Building : MonoBehaviour, ISelectable
{

    public UnityEvent onSelect;
    public UnityEvent onDeselect;

    private BuildingData _config;
    private List<Vector2Int> _cellPositions;
    private SelectionMarker _marker;

    public void Init(BuildingData config)
    {
        _config = config;
    }
    public void Build(Cell cell)
    {
        transform.position = cell.worldPosition + _config.CenterOffset;

        _cellPositions = new();
        for (int x = 0; x < _config.size.x; x++)
        {
            for (int y = 0; y < _config.size.y; y++)
            {
                _cellPositions.Add(new Vector2Int(cell.gridX + x, cell.gridY + y));
            }
        }
        GameInitiator.Instance.pathfindingDirector.grid.UpdateNodesWalkableCell(_cellPositions.ToArray(), false);

    }

    public void Select(SelectionMarker selectionMarker)
    {
        Debug.Log($"{name} selected");
        onSelect.Invoke();

        _marker = selectionMarker;
        _marker.AttachTo(transform, Vector3.zero, _config.size);
    }

    public void Deselect()
    {
        Debug.Log($"{name} deselected");
        onDeselect.Invoke();

        if (_marker) _marker.Detach();
    }

    public void Execute(Vector3 positon)
    {
        throw new System.NotImplementedException();
    }
}
