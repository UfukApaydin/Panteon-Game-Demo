using AStarPathfinding;
using Cysharp.Threading.Tasks;
using Game.Unit;
using GridSystem;
using SelectionSystem;
using SelectionSystem.Marker;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public abstract class BuildingBase : MonoBehaviour, IAttackable
{

    public UnityEvent onSelect;
    public UnityEvent onDeselect;
    public BuildingData Data { get; private set; }
    public Action<int> OnHealthChange { get; set; }


    [SerializeField] private bool _isBuildingConstructed = false;
    private List<Vector2Int> _cellPositions;
    private SelectionMarker _marker;
    private int _currentHealth;
    private Node originNode;
    protected Waypoint _waypoint;
    protected BuildingUIInfo _currentStrategy;
    public GameObject Owner => gameObject;
    public Vector2 Objectsize => Data.size;
    public void Init(BuildingData config)
    {
        Data = config;
        CurrentHealth = config.maxHealth;

        ServiceLocator.Get<ProductionController>().RegisterProduceableUnitRange(Data.unitDatas);
    }
    public void Build(Node node)
    {
        originNode = node;
        transform.position = node.worldPosition + Data.CenterOffset;
        UpdateGridCells(node, false);
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
        ConstructionComplete();
    }
    public abstract void ConstructionComplete();
    public abstract void DestroyBuilding();
    public abstract void Produce(UnitData unitData);

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

  
    public void Select(SelectionMarker selectionMarker)
    {

        onSelect.Invoke();
        _marker = selectionMarker;
        _marker.AttachTo(transform, Vector3.zero, Data.size);
        _waypoint?.ActivateWaypoint();

        _currentStrategy = new BuildingUIInfo(this);
        ServiceLocator.Get<InfoController>().SelectEntity(_currentStrategy);

    }
    public void Deselect()
    {

        onDeselect.Invoke();
        if (_marker) _marker.Detach();
        _waypoint?.DeactivateWaypoint();


        ServiceLocator.Get<InfoController>().DeselectEntity(_currentStrategy);

    }

    public void Command(Vector3 position)
    {
     //   _waypoint?.MoveWaypoint(ServiceLocator.Get<GridManager>().GetSnapPosition(position));
        _waypoint?.MoveWaypoint(ServiceLocator.Get<PathfindingDirector>().grid.NodeFromWorldPoint(position).worldPosition);
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        
        if (CurrentHealth <= 0)
        {

            StartDestroyBuilding();
        }

    }

    private void StartDestroyBuilding()
    {
        _waypoint?.DeactivateWaypoint();
        UpdateGridCells(originNode, true);
        DestroyBuilding();
    }

    /// <summary>
    /// Gets occupied _cells and update _pathfinding _grid _cells.
    /// </summary>
    /// <param name="node">Lower left corner node of the building</param>
    private void UpdateGridCells(Node node, bool isWalkable)
    {
        _cellPositions = new();
        for (int x = 0; x < Data.size.x; x++)
        {
            for (int y = 0; y < Data.size.y; y++)
            {
                _cellPositions.Add(new Vector2Int(node.gridX + x, node.gridY + y));
            }
        }
        ServiceLocator.Get<PathfindingDirector>().grid.UpdateNodesWalkableCell(_cellPositions.ToArray(), isWalkable);

    }

}
