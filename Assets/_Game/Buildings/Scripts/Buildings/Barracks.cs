using AStarPathfinding;
using Cysharp.Threading.Tasks;
using Game.Unit;
using ObjectPoolSystem;
using UnityEngine;

public class Barracks : BuildingBase
{
    public PoolSystem soldierPoolSystem;

    private PathfindingGrid Grid => ServiceLocator.Get<PathfindingDirector>().grid;
    // public UnitData unitData;
    private FactoryManager _factoryManager => ServiceLocator.Get<FactoryManager>();
    [ContextMenu(nameof(Produce))]
    public override async void Produce(UnitData unitData)
    {
        Vector3 spawnPosition = Grid.FindClosestWalkableNode(Grid.NodeFromWorldPoint(transform.position)).worldPosition;
        UnitBase unit = _factoryManager.Create<UnitBase>(soldierPoolSystem,unitData, spawnPosition);
        await UniTask.WaitForSeconds(0.2f);
        unit.Agent.MoveToPosition(_waypoint.GetWaypointPosition());
    }


  

}
