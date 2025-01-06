using A_Pathfinding.Pathfinding;
using Cysharp.Threading.Tasks;
using Game.Unit;
using GridSystem;
using SelectionSystem;
using SelectionSystem.Marker;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BuildingBase : MonoBehaviour, ISelectable
{

    public UnityEvent onSelect;
    public UnityEvent onDeselect;
    public BuildingData Data { get; private set; }
    public Action<int> OnHealthChange;

    [SerializeField] private bool _isBuildingConstructed = false;
    private List<Vector2Int> _cellPositions;
    private SelectionMarker _marker;
    private int _currentHealth;
    protected Waypoint _waypoint;

    public void Init(BuildingData config)
    {
        Data = config;
        CurrentHealth = config.maxHealth;
    
    }
    public void Build(Cell cell)
    {
        transform.position = cell.worldPosition + Data.CenterOffset;
        UpdatePatfindingCells(cell);
        _waypoint = Data.canProduceUnit ? new(Data.waypointPool, transform.position, transform.position) : null;
        ConstructBuilding();
    }

    private async void ConstructBuilding()
    {
        if (Data.buildTime > 0)
        {
            await UniTask.WaitForSeconds(Data.buildTime, cancellationToken: destroyCancellationToken);
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
        for (int x = 0; x < Data.size.x; x++)
        {
            for (int y = 0; y < Data.size.y; y++)
            {
                _cellPositions.Add(new Vector2Int(cell.gridX + x, cell.gridY + y));
            }
        }
        ServiceLocator.Get<PathfindingDirector>().grid.UpdateNodesWalkableCell(_cellPositions.ToArray(), false);

    }
    public int CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        set
        {
            _currentHealth = value;
            OnHealthChange?.Invoke(_currentHealth);

        }
    }
    #region UI
    public abstract void Produce(UnitData unitData);
    
    #endregion

    #region Selection System
    public void Select(SelectionMarker selectionMarker)
    {

        onSelect.Invoke();
        _marker = selectionMarker;
        _marker.AttachTo(transform, Vector3.zero, Data.size);
        _waypoint?.ActivateWaypoint();

        ServiceLocator.Get<ProductionController>().SelectBuilding(this);
       
    }
    public void Deselect()
    {

        onDeselect.Invoke();
        if (_marker) _marker.Detach();
        _waypoint?.DeactivateWaypoint();


        ServiceLocator.Get<ProductionController>().DeselectBuilding(this);

    }

    public void Execute(Vector3 positon)
    {
        _waypoint?.MoveWaypoint(ServiceLocator.Get<GridManager>().GetSnapPosition(positon));
    }
    #endregion
}
