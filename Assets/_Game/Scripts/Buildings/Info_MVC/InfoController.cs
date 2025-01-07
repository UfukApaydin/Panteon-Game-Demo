using Game.Unit;
using SelectionSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InfoController
{
    private readonly InfoView _view;
    private BuildingBase _currentBuilding;

    public InfoController(InfoView view)
    {
        _view = view;
        _view.Init(this);
    }

    InfoUIStrategyBase currentStrategy = null;
    public void SelectEntity(InfoUIStrategyBase data)
    {
        UnsubscribeFromOldStrategy();

         currentStrategy = data;
        _view.UpdateView(currentStrategy);
        if (currentStrategy != null && currentStrategy.TryGetAttackable(out IAttackable attackable))
        {
            attackable.OnHealthChange += _view.UpdateHealthChange;
            _view.UpdateHealthChange(currentStrategy.CurrentHealth);
        }

        void UnsubscribeFromOldStrategy()
        {
            if (currentStrategy != null && currentStrategy.TryGetAttackable(out IAttackable attackable))
            {
                attackable.OnHealthChange -= _view.UpdateHealthChange;
            }

        }
    }
    public void DeselectEntity(InfoUIStrategyBase data)
    {
        if (currentStrategy != null && currentStrategy == data)
        {
          if(  currentStrategy.TryGetAttackable(out IAttackable attackable))
                attackable.OnHealthChange -= _view.UpdateHealthChange;
            _view.ResetView();
        }
    }

}




