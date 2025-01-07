using System.Collections.Generic;
using UnityEngine;

namespace ObjectPoolSystem
{

    public class PoolManager
    {

        private readonly Dictionary<PoolTypeSO, PoolSystem> _pools = new Dictionary<PoolTypeSO, PoolSystem>();

        public void CreatePool(PoolTypeSO poolType)
        {
            if (_pools.ContainsKey(poolType))
            {
                Debug.LogWarning($"Pool for {poolType.poolName} already exists.");
                return;
            }

            var pool = new PoolSystem(poolType.poolPrefab, poolType.defaultPoolSize, poolType.maxPoolSize);
            _pools.Add(poolType, pool);
            Debug.Log($"Created pool for {poolType.poolName}.");
        }
        public IPoolable GetObject(PoolTypeSO poolType)
        {
            if (!_pools.ContainsKey(poolType))
            {
                Debug.LogWarning($"Pool for {poolType} does not exist. Creating a new pool.");
            }

            return _pools[poolType].Get();
        }

        public void ReturnObject(PoolTypeSO poolType, IPoolable obj)
        {
         
            if (_pools.ContainsKey(poolType))
            {
                _pools[poolType].Return(obj);
            }
            else
            {
                Debug.LogError($"No pool found for {poolType.poolName} to return object.");
            }
        }

        public PoolManager CreatePools(PoolTypeSO[] poolTypes)
        {
            foreach (PoolTypeSO poolType in poolTypes)
            {
                CreatePool(poolType);
            }
            return this;
        }
    }
}
