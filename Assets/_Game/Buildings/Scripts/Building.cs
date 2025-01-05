using A_Pathfinding.Pathfinding;
using Cysharp.Threading.Tasks;
using GridSystem;
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

    [SerializeField] private bool _isBuildingConstructed = false; 
    public void Init(BuildingData config)
    {
        _config = config;
    }
    public void Build(Cell cell)
    {
        transform.position = cell.worldPosition + _config.CenterOffset;
        UpdatePatfindingCells(cell);


        ConstructBuilding();

    }

    private async void ConstructBuilding()
    {
        if(_config.buildTime > 0)
        {
          await  UniTask.WaitForSeconds(_config.buildTime, cancellationToken : destroyCancellationToken);
        }
       _isBuildingConstructed = true;
    }
    /// <summary>
    /// Gets occupied _cells and update pathfinding grid _cells.
    /// </summary>
    /// <param name="cell">Lower left corner cell of the building</param>
    private void UpdatePatfindingCells(Cell cell)
    {
        _cellPositions = new();
        for (int x = 0; x < _config.size.x; x++)
        {
            for (int y = 0; y < _config.size.y; y++)
            {
                _cellPositions.Add(new Vector2Int(cell.gridX + x, cell.gridY + y));
            }
        }
        ServiceLocator.Get<PathfindingDirector>().grid.UpdateNodesWalkableCell(_cellPositions.ToArray(), false);

    }

    #region Selection System
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
    #endregion
}
