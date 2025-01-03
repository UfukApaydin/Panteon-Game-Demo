using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingPlacementModel 
{
    public List<BuildingData> buildingConfigs = new();

    public void AddBuildingData(BuildingData buildingConfig)
    {
        buildingConfigs.Add(buildingConfig);
        //Invoke an action for update
    }
    public void AddBuildingDataRange(BuildingData[] buildingConfigs)
    {
       this.buildingConfigs.AddRange(buildingConfigs);
    }

    public void LoadBuildingAddressables()
    {
        // Search for buildings
    }
}
