using Game.Unit;
using SelectionSystem;
using System.Collections.Generic;
using UnityEngine;

public class UnitUIInfo : InfoUIStrategyBase
{
    public UnitData data;
    private UnitBase _unitBase;
    public UnitUIInfo(UnitBase unitBase) 
    {
        _unitBase = unitBase;
        data = _unitBase.Data;
    }
    public override string Name => data.name;
    public override Texture Icon => data.icon;
    public override int CurrentHealth => _unitBase.CurrentHealth;
    public override List<UnitData> GetProductionData => null;
    public override bool TryGetAttackable(out IAttackable attackable)
    {

        if (_unitBase is not IAttackable)
        {
            attackable = null;
            return false;
        }

        attackable = _unitBase as IAttackable;
        return true;
    }
}