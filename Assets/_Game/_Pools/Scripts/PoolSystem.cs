using System;
using UnityEngine;
using UnityEngine.Pool;

namespace SelectionSystem.Marker
{
    [CreateAssetMenu(fileName = "PoolSystem", menuName = "Scriptable Objects/PoolSystem")]

    public class PoolSystem : ScriptableObject
    {
        public GameObject pooledObj;
        public IPoolable poolableObj;

        [SerializeReference] private int _defaultPoolSize = 50;
        [SerializeReference] private int _maxPoolSize = 100;

        private ObjectPool<IPoolable> _pool;
        private Transform _markerPoolParent;

        private void OnValidate()
        {
            if (!pooledObj.TryGetComponent<IPoolable>(out poolableObj))
            {

                throw new InvalidOperationException($"{name} : {pooledObj.name} must have a component implementing {nameof(IPoolable)} for this operation to proceed.");
            }
        }
        public void Init()
        {
            if (_pool == null)
            {
                _markerPoolParent = new GameObject($"{pooledObj.name} Pool").transform;
                _pool = new ObjectPool<IPoolable>(() =>
                {
                    var obj = Instantiate(poolableObj.GameObject, _markerPoolParent).GetComponent<IPoolable>();
                    obj.Construct(this);
                    return obj;
                },
                 poolObject => { poolObject.Activate(); },
                 poolObject => { poolObject.Deactivate(); },
                 poolObject => { Destroy(poolObject.GameObject); },
                 false, _defaultPoolSize, _maxPoolSize
                );
            }
        }

        public IPoolable GetMarker()
        {
            return _pool.Get();
        }

        public void ReturnMarker(IPoolable marker)
        {
            _pool.Release(marker);
        }

    }

    public interface IPoolable
    {
        public GameObject GameObject { get; }
        public void Construct(PoolSystem poolSystem);
        public void Activate();
        public void Deactivate();
    }



}
