using AStarPathfinding;
using Cysharp.Threading.Tasks;
using Game.Unit;
using UnityEngine;

public class Barracks : BuildingBase
{

    private PathfindingGrid Grid => ServiceLocator.Get<PathfindingDirector>().grid;
    // public UnitData buildingDatas;
    private FactoryManager _factoryManager => ServiceLocator.Get<FactoryManager>();
    private ProductionController _buildingPlacementController => ServiceLocator.Get<ProductionController>();

    public override void ConstructionComplete()
    {
        foreach (UnitData unitData in Data.unitDatas)
        {
            unitData.BuildUnitGlobally += Produce;
        }
        _buildingPlacementController.activeBuildings.RegisterBuilding/*<Barracks>*/(typeof(Barracks), this);


    }
    public override void DestroyBuilding()
    {
        foreach (UnitData unitData in Data.unitDatas)
        {
            unitData.BuildUnitGlobally -= Produce;
        }
        _buildingPlacementController.activeBuildings.RemoveBuilding/*<Barracks>*/(typeof(Barracks), this);
        Destroy(gameObject);
    }

    public override async void Produce(UnitData unitData)
    {
        Vector3 spawnPosition = Grid.FindClosestWalkableNode(Grid.NodeFromWorldPoint(transform.position)).worldPosition;
        UnitBase unit = _factoryManager.Create<UnitBase>(unitData, spawnPosition);
        await UniTask.WaitForSeconds(0.2f);
        unit.Agent.MoveToPosition(_waypoint.GetWaypointPosition());
    }




}
