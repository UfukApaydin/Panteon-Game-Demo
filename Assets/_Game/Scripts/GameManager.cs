using AStarPathfinding;
using ObjectPoolSystem;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Configs")]
    public GameData gameData;

    [Header("From Scene")]
    public ProductionView buildingPlacementView;
    public InfoView productionView;
    public static GameManager Instance { get; private set; }

    private void OnValidate()
    {
        if (gameData.gridConfig == null) throw new System.Exception($"{nameof(gameData.gridConfig)} is not assigned in {gameObject.name}.");

        if (gameData.buildSystemConfig == null) throw new System.Exception($"{nameof(gameData.buildSystemConfig)} is not assigned in {gameObject.name}.");
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

        ServiceLocator.Register(new PoolManager()
            .CreatePools(new PoolTypeSO[]
            {   gameData.markerType,
                gameData.waypointType,
                gameData.soldierType
            }));
        ServiceLocator.Register(new FactoryManager().InitializeFactories());
        ServiceLocator.Register(new PathfindingDirector()
            .InitializePathfinding(gameData.gridConfig));
        ServiceLocator.Register(new ProductionController.Builder()
            .WithBuildings(gameData.buildSystemConfig.buildingDatas)
            .BuildAndStart(buildingPlacementView, gameData.buildSystemConfig));
        ServiceLocator.Register(new InfoController(productionView));

        Camera.main.AddComponent<CameraController>().Init(gameData.gridConfig);

    }

}
