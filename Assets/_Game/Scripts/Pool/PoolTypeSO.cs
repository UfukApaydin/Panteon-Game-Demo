using UnityEngine;

namespace ObjectPoolSystem
{
    [CreateAssetMenu(fileName = "PoolTypeSO", menuName = "Scriptable Objects/Pool/PoolTypeSO")]
    public class PoolTypeSO : ScriptableObject
    {
        public string poolName;
        public GameObject poolPrefab;
        public int defaultPoolSize = 50;
        public int maxPoolSize = 100;
    }
}