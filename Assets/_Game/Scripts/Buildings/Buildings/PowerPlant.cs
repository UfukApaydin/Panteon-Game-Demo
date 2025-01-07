using Game.Unit;
using UnityEngine;

public class PowerPlant : BuildingBase
{
    private ProductionController _buildingPlacementController => ServiceLocator.Get<ProductionController>();

    public override void ConstructionComplete()
    {
        _buildingPlacementController.activeBuildings.RegisterBuilding/*<PowerPlant>*/(typeof(PowerPlant) ,this);
    }
    public override void DestroyBuilding()
    {
        _buildingPlacementController.activeBuildings.RemoveBuilding/*<PowerPlant>*/(typeof(PowerPlant), this);
        Destroy(gameObject);
    }
    public override void Produce(UnitData unitData)
    {
      
    }
    
}
