using UnityEngine;

public class BuildingPreviewFactory : BaseFactory<BuildingPreview>
{
    public override BuildingPreview Create(params object[] args)
    {
        BuildingData config = args[0].VerifyType<BuildingData>();

        var previewObj = new GameObject($"{config.buildingName}_Preview");
        var preview = previewObj.AddComponent<BuildingPreview>();
        var renderer = previewObj.AddComponent<SpriteRenderer>();

        preview.transform.localScale = new Vector3(config.size.x, config.size.y, 1);
        renderer.sprite = config.visual;
        renderer.color = config.placementConfig.buildingPreviewTint;
        renderer.sortingOrder = 2;
        return preview;
    }
}
