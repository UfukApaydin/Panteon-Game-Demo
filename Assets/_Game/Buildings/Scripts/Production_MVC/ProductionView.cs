using Game.Unit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionView : MonoBehaviour
{
    public InfiniteScrollUI InfiniteScrollUI;
    private ProductionController _productionController;

    public void Init(ProductionController buildingController)
    {
        _productionController = buildingController;

    }
    public void CreateBuildingButtons(BuildingData[] buildings)
    {
        InfiniteScrollUI.SetUIData(new BuildingUIStrategy(_productionController, buildings));

    }

    public void CreateSoldierButtons(UnitData[] units)
    {
        InfiniteScrollUI.SetUIData(new UnitUIStrategy(_productionController, units));

    }
    public void SelectBuildingTab()
    {
        _productionController.RequestBuildingData();
    }
    public void SelectSoldierTab()
    {
        _productionController.RequestSoldierData();
    }
}
