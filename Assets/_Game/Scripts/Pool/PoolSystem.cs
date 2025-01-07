using System;
using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPoolSystem
{
    public class PoolSystem
    {
        public GameObject PooledObj { get; private set; }
        public IPoolable PoolableObj { get; private set; }

        private int _defaultPoolSize;
        private int _maxPoolSize;

        private ObjectPool<IPoolable> _pool;
        private Transform _poolParent;

        public PoolSystem(GameObject pooledObj, int defaultPoolSize = 50, int maxPoolSize = 100)
        {
            if (!pooledObj.TryGetComponent<IPoolable>(out var poolableObj))
            {
                throw new InvalidOperationException($"{pooledObj.name} must have a component implementing {nameof(IPoolable)}.");
            }

            PooledObj = pooledObj;
            PoolableObj = poolableObj;
            _defaultPoolSize = defaultPoolSize;
            _maxPoolSize = maxPoolSize;

            Init();
        }

        private void Init()
        {
            if (_pool == null)
            {
                _poolParent = new GameObject($"{PooledObj.name} Pool").transform;
                _pool = new ObjectPool<IPoolable>(() =>
                {
                    var obj = UnityEngine.Object.Instantiate(PooledObj, _poolParent).GetComponent<IPoolable>();
                    obj.Init();
                    return obj;
                },
                poolObject => poolObject.Activate(),
                poolObject => poolObject.Deactivate(),
                poolObject => UnityEngine.Object.Destroy(poolObject.GameObject),
                false, _defaultPoolSize, _maxPoolSize);
            }
        }

        public IPoolable Get()
        {
            return _pool.Get();
        }

        public void Return(IPoolable poolObj)
        {
            _pool.Release(poolObj);
        }
    }
}
