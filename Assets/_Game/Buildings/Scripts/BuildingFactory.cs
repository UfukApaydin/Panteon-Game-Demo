using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.STP;

public interface IBuildingFactory
{
    Building Create(BuildingConfig config);
}
public class BuildingFactory : IBuildingFactory
{
   public Building Create(BuildingConfig config)
    {
        GameObject building = Object.Instantiate(config.prefab);
        Building buildingComponent = building.GetComponent<Building>();
        buildingComponent.Init(config);
        return buildingComponent;
    }
}
