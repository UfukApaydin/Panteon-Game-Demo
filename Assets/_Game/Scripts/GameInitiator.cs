using A_Pathfinding.Pathfinding;
using GridSystem;
using UnityEngine;

public class GameInitiator : MonoBehaviour
{
    [Header("Configs")]
    public GridConfig gridConfig;
    public BuildSystemConfig buildSystemConfig;

    [Header("From Scene")]
    public BuildingPlacementView buildingPlacementView;
    public ProductionView productionView;

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
        ServiceLocator.Register(new FactoryManager().InitializeFactories());
        ServiceLocator.Register(new GridManager(gridConfig));
        ServiceLocator.Register(new PathfindingDirector()
            .InitializePathfinding(gridConfig));
        ServiceLocator.Register(new BuildingPlacementController.Builder()
            .WithBuildings(buildSystemConfig.buildingDatas)
            .BuildAndStart(buildingPlacementView, buildSystemConfig));
        ServiceLocator.Register(new ProductionController(productionView));
    }

}
