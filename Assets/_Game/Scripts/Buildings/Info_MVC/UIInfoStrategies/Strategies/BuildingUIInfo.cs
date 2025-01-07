using Game.Unit;
using SelectionSystem;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BuildingUIInfo : InfoUIStrategyBase
{
    public BuildingData data;
    private BuildingBase _buildingBase;
    public BuildingUIInfo(BuildingBase buildingBase) 
    {
        _buildingBase = buildingBase;
        data = _buildingBase.Data;
    }
    public override string Name => data.name;
    public override Texture Icon => data.icon;
    public override int CurrentHealth => _buildingBase.CurrentHealth;
    public override List<UnitData> GetProductionData => new List<UnitData>(data.unitDatas);

    public override bool TryGetAttackable(out IAttackable attackable)
    {

        if (_buildingBase is not IAttackable)
        {
            attackable = null;
            return false;
        }

        attackable = _buildingBase as IAttackable;
        return true;
    }
}
