using Game.Unit;
using ObjectPoolSystem;
public class UnitFactory : BaseFactory<UnitBase>
{

    private PoolManager poolManager => ServiceLocator.Get<PoolManager>();
    /// <summary>
    ///  1 - UnitData
    ///  2 - Vector3 spawnPosition
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public override UnitBase Create(params object[] args)
    {
        UnitData config = args[0].VerifyType<UnitData>();

        IPoolable poolableObj = poolManager.GetObject(GameManager.Instance.gameData.soldierType);
        poolableObj.UpdateArgs(config, args[1]);
        return poolableObj.GameObject.GetComponent<UnitBase>();
    }


}
