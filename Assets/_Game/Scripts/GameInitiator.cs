using A_Pathfinding.Nodes;
using A_Pathfinding.Pathfinding;
using A_Pathfinding.Test;
using TileSystem;
using UnityEngine;
using PathfindingGrid = A_Pathfinding.Nodes.PathfindingGrid;

public class GameInitiator : MonoBehaviour
{
    [Header("Bindings")]
    public PathfindingGrid grid;
    public GridManager gridManager;
    public PointClick pointClick;
    public Pathfinding pathfinding;
    [Header("Bindings Prefab")]
    [SerializeField] private PathfindingGrid gridPrefab;
    [SerializeField] private GridManager gridManagerPrefab;
    [SerializeField] private PointClick pointClickPrefab;
    [SerializeField] private Pathfinding pathfindingPrefab;
    [Header("Settings")]
    [Header("Grid")]
    public Vector2Int gridWorldSize;
    public float nodeRadius;

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
        grid = Instantiate(gridPrefab, transform);
        pointClick = Instantiate(pointClickPrefab, transform);
        pathfinding = Instantiate(pathfindingPrefab, transform);
    }
    // Initialize 
    private void Initialize()
    {
        gridManager.Init(gridWorldSize, nodeRadius);
        grid.Init(gridWorldSize, nodeRadius/2);
        pointClick.Init(gridWorldSize, nodeRadius);
        pathfinding.Init(grid);
    }

    //Prepare

    //Start Game
}
