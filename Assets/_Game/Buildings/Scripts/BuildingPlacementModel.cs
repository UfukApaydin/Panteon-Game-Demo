using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacementModel : MonoBehaviour
{
    public List<BuildingConfig> buildingConfigs;

    public void AddBuildingData(BuildingConfig buildingConfig)
    {
        buildingConfigs.Add(buildingConfig);
        //Invoke an action for update
    }
}
