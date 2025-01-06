using Cysharp.Threading.Tasks;
using Game.Unit;
using UnityEngine;

public class Barracks : BuildingBase
{

   // public UnitData unitData;
    private FactoryManager _factoryManager => ServiceLocator.Get<FactoryManager>();
    [ContextMenu(nameof(Produce))]
    public override async void Produce(UnitData unitData)
    {
        UnitBase unit = _factoryManager.Create<UnitBase>(unitData);
        await UniTask.WaitForSeconds(0.2f);
        unit.Agent.MoveToPosition(_waypoint.GetWaypointPosition());
    }
  

}
