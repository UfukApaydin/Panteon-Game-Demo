using AStarPathfinding;
using GridSystem;
using UnityEngine;

public class GameInitiator : MonoBehaviour
{
    [Header("Configs")]
    public GridConfig gridConfig;
    public BuildSystemConfig buildSystemConfig;

    [Header("From Scene")]
    public ProductionView buildingPlacementView;
    public InfoView productionView;

    public static GameInitiator Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
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
        ServiceLocator.Register(new ProductionController.Builder()
            .WithBuildings(buildSystemConfig.buildingDatas)
            .BuildAndStart(buildingPlacementView, buildSystemConfig));
        ServiceLocator.Register(new InfoController(productionView));
    
    }

}
