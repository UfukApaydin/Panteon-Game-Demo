using System.Collections.Generic;
using UnityEngine;

public interface IBuildingFactory
{
    Building Create(BuildingData config);

    GameObject CreatePreview(BuildingData config); 
}
public class BuildingFactory : IBuildingFactory
{
   public Building Create(BuildingData config)
    {
        GameObject building = Object.Instantiate(config.prefab);
        Building buildingComponent = building.GetComponent<Building>();
        buildingComponent.Init(config);
        return buildingComponent;
    }
    public GameObject CreatePreview(BuildingData config)
    {
        GameObject preview = new GameObject($"{config.buildingName}_Preview");
        preview.transform.localScale = new Vector3(config.size.x, config.size.y, 1);
        var renderer = preview.AddComponent<SpriteRenderer>();
        renderer.sprite = config.visual;
        renderer.color = config.placementConfig.buildingPreviewTint;
        renderer.sortingOrder = 2;
        return preview;

    }
}
