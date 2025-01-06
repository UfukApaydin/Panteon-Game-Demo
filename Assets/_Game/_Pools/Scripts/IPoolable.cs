using UnityEngine;

namespace ObjectPoolSystem
{
    public interface IPoolable
    {
        public GameObject GameObject { get; }
        public void Init(PoolSystem poolSystem);
        public void Activate();
        public void Deactivate();
        public void UpdateArgs(params object[] args);
    }



}
