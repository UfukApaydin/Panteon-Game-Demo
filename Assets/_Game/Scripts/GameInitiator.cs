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
    public BuildingPlacementController buildingPlacementController;

    [Header("From Scene")]
    public BuildingPlacementView buildingPlacementView;

    [Header("Bindings Prefab")]
    [SerializeField] private GridManager gridManagerPrefab;
    [SerializeField] private PointClick pointClickPrefab;

    [Header("Settings")]
    [Header("Grid")]
    public Vector2Int gridWorldSize;
    public float nodeRadius;
    public LayerMask unwalkableLayerMask;
    [Header("Buildings")]
    public BuildingData[] buildingDatas;
    public LayerMask groundLayerMask;

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

    private void Initialize()
    {
        pathfindingDirector = new PathfindingDirector()
            .InitializePathfinding(gridWorldSize, nodeRadius / 2, unwalkableLayerMask);
        gridManager.Init(gridWorldSize, nodeRadius);
        pointClick.Init(gridWorldSize, nodeRadius);
        buildingPlacementController = new BuildingPlacementController.Builder()
            .WithBuildings(buildingDatas)
            .BuildAndStart(buildingPlacementView,groundLayerMask);
    }

    //Prepare

    //Start Game
}
