using A_Pathfinding.Pathfinding;
using GridSystem;
using UnityEngine;

public class GameInitiator : MonoBehaviour
{
    [Header("Configs")]
    public GridConfig gridConfig;

    [Header("From Scene")]
    public BuildingPlacementView buildingPlacementView;


    [Header("Settings")]
    [Header("Grid")]
    public Vector2Int gridWorldSize;
    public float nodeRadius;
    public LayerMask unwalkableLayerMask;
    [Header("Buildings")]
    public BuildingData[] buildingDatas;
    public LayerMask groundLayerMask;
    public LayerMask placeableLayerMask;

    public static GameInitiator Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
        Init();
    }


    private void Init()
    {
        ServiceLocator.Register(new GridManager(gridConfig));
        ServiceLocator.Register(new PathfindingDirector()
            .InitializePathfinding(gridWorldSize, nodeRadius / 2, unwalkableLayerMask));
        ServiceLocator.Register(new BuildingPlacementController.Builder()
            .WithBuildings(buildingDatas)
            .BuildAndStart(buildingPlacementView, groundLayerMask, placeableLayerMask));

    }

}
