using Game.Unit;
using SelectionSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class InfoUIStrategyBase
{

    public abstract string Name { get; }
    public abstract Texture Icon { get; }

    public abstract int CurrentHealth {  get; }

    public abstract List<UnitData> GetProductionData { get; }

    public abstract bool TryGetAttackable(out IAttackable attackable);

}