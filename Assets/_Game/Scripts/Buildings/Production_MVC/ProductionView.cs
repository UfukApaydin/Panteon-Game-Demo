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

    public void PopulateUI(UIStrategyBase strategy)
    {
        InfiniteScrollUI.SetUIData(strategy);
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
