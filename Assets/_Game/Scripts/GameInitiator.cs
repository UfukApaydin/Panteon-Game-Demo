using AStarPathfinding;
using GridSystem;
using Unity.VisualScripting;
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

    private void OnValidate()
    {
        if (gridConfig == null) throw new System.Exception($"{nameof(gridConfig)} is not assigned in {gameObject.name}.");

        if (buildSystemConfig == null) throw new System.Exception($"{nameof(buildSystemConfig)} is not assigned in {gameObject.name}.");
    }

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
        ServiceLocator.Register(new PathfindingDirector()
            .InitializePathfinding(gridConfig));
        ServiceLocator.Register(new ProductionController.Builder()
            .WithBuildings(buildSystemConfig.buildingDatas)
            .BuildAndStart(buildingPlacementView, buildSystemConfig));
        ServiceLocator.Register(new InfoController(productionView));

        Camera.main.AddComponent<CameraController>().Init(gridConfig);

    }

}
