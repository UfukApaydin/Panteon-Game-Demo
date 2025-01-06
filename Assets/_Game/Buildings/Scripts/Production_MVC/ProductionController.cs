using Game.Unit;

public class ProductionController
{
    private readonly ProductionView _view;
    private BuildingBase _currentBuilding;

    public ProductionController(ProductionView view)
    {
        _view = view;
        _view.Init(this);
    }
    public void SelectBuilding(BuildingBase newBuilding)
    {
        if (_currentBuilding != null)
        {
            _currentBuilding.OnHealthChange -= _view.UpdateHealthChange;
        }

        _currentBuilding = newBuilding;
        _view.UpdateView(_currentBuilding.Data);
        if (_currentBuilding != null)
        {
            _currentBuilding.OnHealthChange += _view.UpdateHealthChange;
            _view.UpdateHealthChange(_currentBuilding.CurrentHealth);
        }
    }
    public void DeselectBuilding(BuildingBase building)
    {
        if (_currentBuilding != null && _currentBuilding == building)
        {
            _view.ResetView();
        }
    }
    public void StartProduction(UnitData unitData)
    {
        if (_currentBuilding != null)
            _currentBuilding.Produce(unitData);
    }
}
