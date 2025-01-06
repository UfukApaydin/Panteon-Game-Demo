using System.Threading.Tasks;
using UnityEngine;
using Game.Unit;
public class UnitFactory : BaseFactory<UnitBase>
{
    public override UnitBase Create(params object[] args)
    {
        UnitData config = args[0].VerifyType<UnitData>();

        GameObject unit = Object.Instantiate(config.prefab);
        UnitBase unitComponent = unit.GetComponent<UnitBase>();
        unitComponent.Init(config);
        return unitComponent;
    }

    
}
