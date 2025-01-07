using Game.Unit;
using GridSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ProductionController : MonoBehaviour
{
    public BuildSystemConfig buildSystemConfig;
    public ProductionView productionView;
    public ProductionModel productionModel;
    public ActiveBuildings activeBuildings;

    private Camera _mainCamera;
    private BuildingData _selectedBuilding = null;
    private GameObject _previewInstance;
    private Vector3 _buildingOffset;
    private UIStrategyBase _activeStrategy;
    private FactoryManager factoryManager => ServiceLocator.Get<FactoryManager>();

    private void OnDestroy()
    {
        activeBuildings.onBuildingTypeAdded -= OnBuildingTypeAdded;
        activeBuildings.onBuildingTypeRemoved -= OnBuildingTypeRemoved;
    }

    private void Awake()
    {
        _mainCamera = Camera.main;
        activeBuildings = new ActiveBuildings();

        activeBuildings.onBuildingTypeAdded += OnBuildingTypeAdded;
        activeBuildings.onBuildingTypeRemoved += OnBuildingTypeRemoved;
    }

    private void StartController()
    {
        productionView.Init(this);

        RequestBuildingData();
    }

    public void RequestBuildingData()
    {
        _activeStrategy = new BuildingUIStrategy(this, productionModel.buildingConfigs.ToArray());
        productionView.PopulateUI(_activeStrategy);
    }
    public void RequestSoldierData()
    {
        _activeStrategy = new UnitUIStrategy(this, produceableUnits.ToArray());
        productionView.PopulateUI(_activeStrategy);
    }

    #region Building Production
    public void SelectBuilding(BuildingData buildingData)
    {
        _selectedBuilding = buildingData;
        if (_previewInstance != null)
        {
            Destroy(_previewInstance);
        }
        _buildingOffset = buildingData.CenterOffset;
        _previewInstance = factoryManager.Create<BuildingPreview>(new[] { buildingData }).gameObject;

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

        factoryManager.Create<BuildingBase>(new[] { _selectedBuilding })
             .Build(ServiceLocator.Get<GridManager>().GetCellFromWorlPosition(position));
        Destroy(_previewInstance);
        _previewInstance = null;
        _selectedBuilding = null;
    }
    public Vector3 SnapToGrid(Vector3 position)
    {
        var pos = ServiceLocator.Get<GridManager>().GetSnapPosition(position);
        return new Vector3(pos.x, pos.y, 0);

    }
    #endregion

    #region Soldier Production 

    public void ProduceSoldier(UnitData unitData)
    {
        unitData.BuildUnitGlobally?.Invoke(unitData);
      
    }
    private List<UnitData> produceableUnits = new();

    public void OnBuildingTypeAdded(BuildingData buildingData)
    {
        RegisterProduceableUnitRange(buildingData.unitDatas);
    }
    public void OnBuildingTypeRemoved(BuildingData buildingData)
    {
        RemoveProduceableUnitRange(buildingData.unitDatas);
    }
    private void RegisterProduceableUnit(UnitData unitData)
    {
        if (produceableUnits.Contains(unitData))
            return;
        produceableUnits.Add(unitData);
    }
    public void RegisterProduceableUnitRange(List<UnitData> unitDatas)
    {
        foreach (var unitData in unitDatas)
        {
            RegisterProduceableUnit(unitData);
        }
        if(_activeStrategy != null && _activeStrategy is UnitUIStrategy)
        {
            RequestSoldierData();
        }
    }
    private void RemoveProduceableUnit(UnitData unitData)
    {
        if (produceableUnits.Contains(unitData))
            produceableUnits.Remove(unitData);
    }
    public void RemoveProduceableUnitRange(List<UnitData> unitDatas)
    {
        foreach (var unitData in unitDatas)
        {
            RemoveProduceableUnit(unitData);
        }
        if (_activeStrategy != null && _activeStrategy is UnitUIStrategy)
        {
            RequestSoldierData();
        }
    }
    #endregion


    #region Builder
    public class Builder
    {
        readonly ProductionModel model = new();
        private bool isBuildingProvided = false;
        public Builder WithBuildings(BuildingData[] buildings)
        {
            model.AddBuildingDataRange(buildings);
            isBuildingProvided = true;
            return this;
        }
        public ProductionController Build(ProductionView view, BuildSystemConfig buildSystemConfig)
        {
            if (view != null)
            {
                if (isBuildingProvided)
                    model.LoadBuildingAddressables();
                var controller = new GameObject("ProductionController").AddComponent<ProductionController>();
                controller.productionView = view;
                controller.productionModel = model;
                controller.buildSystemConfig = buildSystemConfig;
                return controller;

            }
            else
                throw new InvalidOperationException("Controller controller cannot be null");
        }
        public ProductionController BuildAndStart(ProductionView view, BuildSystemConfig buildSystemConfig)
        {
            var controller = Build(view, buildSystemConfig);
            controller.StartController();
            return controller;
        }
    }

    #endregion
}
