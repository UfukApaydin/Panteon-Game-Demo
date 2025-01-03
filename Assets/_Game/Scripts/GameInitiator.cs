using A_Pathfinding.Nodes;
using A_Pathfinding.Pathfinding;
using A_Pathfinding.Test;
using TileSystem;
using UnityEngine;
using PathfindingGrid = A_Pathfinding.Nodes.PathfindingGrid;

public class GameInitiator : MonoBehaviour
{
    [Header("Bindings")]
    public GridManager gridManager;
    public PointClick pointClick;
    public PathfindingDirector pathfindingDirector;
    [Header("Bindings Prefab")]
    [SerializeField] private GridManager gridManagerPrefab;
    [SerializeField] private PointClick pointClickPrefab;
       [Header("Settings")]
    [Header("Grid")]
    public Vector2Int gridWorldSize;
    public float nodeRadius;
    public LayerMask unwalkableLayerMask;

    public static GameInitiator Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
        BindObjects();
        Initialize();
    }

    private void BindObjects()
    {
        gridManager = Instantiate(gridManagerPrefab, transform);
        pointClick = Instantiate(pointClickPrefab, transform);
    }
    // Initialize 
    private void Initialize()
    {
        pathfindingDirector = new PathfindingDirector().InitializePathfinding(gridWorldSize, nodeRadius / 2, unwalkableLayerMask);
        gridManager.Init(gridWorldSize, nodeRadius);
    //    grid.Init(gridWorldSize, nodeRadius/2);
        pointClick.Init(gridWorldSize, nodeRadius);
    //    pathfinding.Init(grid);
    }

    //Prepare

    //Start Game
}
