using System.Threading.Tasks;
using UnityEngine;
using Game.Unit;
using ObjectPoolSystem;
public class UnitFactory : BaseFactory<UnitBase>
{/// <summary>
///  0 - PoolSystem
///  1 - UnitData
///  2 - Vector3 spawnPosition
/// </summary>
/// <param name="args"></param>
/// <returns></returns>
    public override UnitBase Create(params object[] args)
    {
        PoolSystem poolSystem =args[0].VerifyType<PoolSystem>();
        UnitData config = args[1].VerifyType<UnitData>();

        poolSystem.Init();
        IPoolable poolableObj = poolSystem.Get();
        poolableObj.UpdateArgs(config, args[2]);
        return poolableObj.GameObject.GetComponent<UnitBase>();
    }

    
}
