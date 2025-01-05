using GridSystem;
using System;
using UnityEngine;

public class BuildingPlacementController : MonoBehaviour
{

    public BuildSystemConfig buildSystemConfig;

    public BuildingPlacementView placementView;
    public BuildingPlacementModel placementModel;

    private Camera _mainCamera;
    private BuildingFactory _buildingFactory;
    private BuildingData _selectedBuilding = null;
    private GameObject _previewInstance;
    private Vector3 _buildingOffset;


    private void Awake()
    {
        _mainCamera = Camera.main;
        _buildingFactory = new BuildingFactory();
    }

    private void StartController()
    {
        placementView.Initialize(this, placementModel.buildingConfigs.ToArray());
    }


    public void SelectBuilding(BuildingData buildingData)
    {
        _selectedBuilding = buildingData;
        if (_previewInstance != null)
        {
            Destroy(_previewInstance);
        }
        _buildingOffset = buildingData.CenterOffset;
        _previewInstance = _buildingFactory.CreatePreview(buildingData);

    }

    private void Update()
    {
        if (_selectedBuilding != null && _previewInstance != null)
        {
            if (GetMousePosition(out Vector3 mousePosition))
            {
                if (UpdatePreviewInstance(mousePosition))
                {
                    HandleBuildingPlacement(mousePosition);
                }


            }

        }
    }
    private bool GetMousePosition(out Vector3 position)
    {
        Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseWorldPos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos2D, Vector2.zero, 0f, buildSystemConfig.groundLayerMask);
        if (hit.collider != null)
        {
            position = SnapToGrid(hit.point);
            return true;
        }
        position = Vector3.zero;
        return false;
    }
    private bool UpdatePreviewInstance(Vector3 position)
    {
        _previewInstance.transform.position = position + _buildingOffset;

        Collider2D overlapCollider = Physics2D.OverlapBox(_previewInstance.transform.position, _selectedBuilding.size - Vector2.one, 0, buildSystemConfig.previewCollideLayers);
        if (overlapCollider != null)
        {
            _previewInstance.GetComponent<SpriteRenderer>().color = _selectedBuilding.placementConfig.buildingPreviewRestrictedTint;
            return false;
        }
        else
        {
            _previewInstance.GetComponent<SpriteRenderer>().color = _selectedBuilding.placementConfig.buildingPreviewTint;
            return true;
        }

    }
    private void HandleBuildingPlacement(Vector3 position)
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceBuilding(position);
        }

    }

    private void PlaceBuilding(Vector3 position)
    {

        _buildingFactory.Create(_selectedBuilding).Build(ServiceLocator.Get<GridManager>().GetCellFromWorlPosition(position));

        Destroy(_previewInstance);
        _previewInstance = null;
        _selectedBuilding = null;
    }
    public Vector3 SnapToGrid(Vector3 position)
    {
        var pos = ServiceLocator.Get<GridManager>().GetSnapPosition(position); 
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
        public BuildingPlacementController Build(BuildingPlacementView view, BuildSystemConfig buildSystemConfig)
        {
            if (view != null)
            {
                if (isBuildingProvided)
                    model.LoadBuildingAddressables();
                var controller = new GameObject("BuildingPlacementController").AddComponent<BuildingPlacementController>();
                controller.placementView = view;
                controller.placementModel = model;
                controller.buildSystemConfig = buildSystemConfig;
                return controller;

            }
            else
                throw new InvalidOperationException("Controller controller cannot be null");
        }
        public BuildingPlacementController BuildAndStart(BuildingPlacementView view, BuildSystemConfig buildSystemConfig)
        {
            var controller = Build(view, buildSystemConfig);
            controller.StartController();
            return controller;
        }
    }

    #endregion
}
