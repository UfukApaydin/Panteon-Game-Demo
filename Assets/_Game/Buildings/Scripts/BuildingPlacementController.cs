using System;
using UnityEngine;

public class BuildingPlacementController : MonoBehaviour
{
    public BuildingPlacementView placementView;
    public BuildingPlacementModel placementModel;

    public LayerMask groundLayer;

    private BuildingData selectedBuilding = null;
    private GameObject previewInstance;
    private BuildingFactory buildingFactory;
    private Camera mainCamera;

    //private BuildingPlacementController(BuildingPlacementView placementView, BuildingPlacementModel placementModel, LayerMask groundLayer)
    //{
    //    this.placementView = placementView;
    //    this.placementModel = placementModel;
    //    this.groundLayer = groundLayer;
    //    mainCamera = Camera.main;
    //    buildingFactory = new BuildingFactory();

    //}
    private void Awake()
    {
        mainCamera = Camera.main;
        buildingFactory = new BuildingFactory();
    }

    private void StartController()
    {
        placementView.Initialize(this,placementModel.buildingConfigs.ToArray());
    }


    public void SelectBuilding(BuildingData buildingData)
    {
        selectedBuilding = buildingData;
        if (previewInstance != null)
        {
            Destroy(previewInstance);
        }
        previewInstance = buildingFactory.CreatePreview(buildingData);

    }

    private void Update()
    {
        if (selectedBuilding != null /*&& previewInstance != null*/)
        {
            HandleBuildingPlacement();
        }
    }

    private void HandleBuildingPlacement()
    {

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseWorldPos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos2D, Vector2.zero, 0f, groundLayer);
        if (hit.collider != null)
        {
            Vector3 snappedPosition = SnapToGrid(hit.point);
            previewInstance.transform.position = snappedPosition;

            // check if building can be placed;

            if (Input.GetMouseButtonDown(0)) // Left-click to place
            {
                PlaceBuilding(snappedPosition);
            }
        }
    }

    private void PlaceBuilding(Vector3 position)
    {

        buildingFactory.Create(selectedBuilding).Build(GameInitiator.Instance.gridManager.GetCellFromWorlPosition(position)); 

        Destroy(previewInstance);                                                                                                                                                                                                       
        previewInstance = null;
        selectedBuilding = null;
    }
    public Vector3 SnapToGrid(Vector3 position)
    {
        var pos = GameInitiator.Instance.gridManager.GetSnapPosition(position);  //----------------------Change This-------------------
        return new Vector3(pos.x, pos.y, 0);

    }
    #region Builder
    public class Builder
    {
        readonly BuildingPlacementModel model = new();
        private bool isBuildingProvided = false;
        public Builder WithBuildings(BuildingData[] buildings)
        {
            model.AddBuildingDataRange(buildings);
            isBuildingProvided = true;
            return this;
        }
        public BuildingPlacementController Build(BuildingPlacementView view, LayerMask groundLayerMask)
        {
            if (view != null)
            {
                if (isBuildingProvided)
                    model.LoadBuildingAddressables();
                var controller = new GameObject("BuildingPlacementController").AddComponent<BuildingPlacementController>();
                controller.placementView = view;
                controller.placementModel = model;
                controller.groundLayer = groundLayerMask;
                return controller;

            }
            else
                throw new InvalidOperationException("Controller controller cannot be null");
        }
        public BuildingPlacementController BuildAndStart(BuildingPlacementView view, LayerMask groundLayerMask)
        {
            var controller = Build(view, groundLayerMask);
            controller.StartController();
            return controller;
        }
    }

    #endregion
}
