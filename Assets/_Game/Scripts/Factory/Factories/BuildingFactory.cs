using UnityEngine;



public class BuildingFactory : BaseFactory<BuildingBase>
{
    public override BuildingBase Create(params object[] args)
    {
        BuildingData config = args[0].VerifyType<BuildingData>();

        GameObject building = Object.Instantiate(config.prefab);
        BuildingBase buildingComponent = building.GetComponent<BuildingBase>();
        buildingComponent.Init(config);
        return buildingComponent;
    }

}
