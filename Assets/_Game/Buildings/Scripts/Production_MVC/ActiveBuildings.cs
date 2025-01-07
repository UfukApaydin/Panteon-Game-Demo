using System;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBuildings
{
    public Dictionary<Type, List<BuildingBase>> activeBuildings = new();
    public Action<BuildingData> onBuildingTypeAdded;
    public Action<BuildingData> onBuildingTypeRemoved;
    public void RegisterBuilding<T>(BuildingBase buildingBase)
    {
        var type = typeof(T);
        if (activeBuildings.ContainsKey(type))
        {
            activeBuildings[type].Add(buildingBase);
            Debug.Log($" new building added {buildingBase.gameObject.name} ");
            return;
        }
        List<BuildingBase> buildingBases = new() { buildingBase };
        activeBuildings.Add(type, buildingBases);

        Debug.Log($" new type add {buildingBase.Data.dataName} ");
        onBuildingTypeAdded?.Invoke(buildingBase.Data);
    }

    public void RemoveBuilding<T>(BuildingBase buildingBase)
    {
        var type = typeof(T);
        if (!activeBuildings.ContainsKey(type))
        {
            Debug.Log(" no type found ");
            return;
        }
        List<BuildingBase> buildingBases = activeBuildings[type];
        if (buildingBases.Contains(buildingBase))
            buildingBases.Remove(buildingBase);
        if (buildingBases.Count == 0)
        {
            Debug.Log($" {buildingBase.Data.dataName} ");
            onBuildingTypeRemoved?.Invoke(buildingBase.Data);
        }
        activeBuildings.Remove(type);
    }
}