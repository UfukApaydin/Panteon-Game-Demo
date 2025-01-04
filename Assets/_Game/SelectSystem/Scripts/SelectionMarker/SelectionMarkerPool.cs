using UnityEngine;
using UnityEngine.Pool;

namespace SelectionSystem.Marker
{
    [CreateAssetMenu(fileName = "SelectionMarkerPool", menuName = "Scriptable Objects/Options/SelectionMarkerPool")]
    public class SelectionMarkerPool : ScriptableObject
    {
        public SelectionMarker selectionMarkerPrefab;

        [SerializeReference]private int _defaultPoolSize = 50;
        [SerializeReference]private int _maxPoolSize = 100;

        private ObjectPool<SelectionMarker> _pool;
        private Transform _markerPoolParent;

        public void Init()
        {
            if (_pool == null)
            {
                _markerPoolParent = new GameObject("Selection Marker Pool").transform;
                _pool = new ObjectPool<SelectionMarker>(() =>
                {
                    var obj = Instantiate(selectionMarkerPrefab, _markerPoolParent);
                    obj.Construct(this);
                    return obj ;
                },
                 markerObj => { markerObj.Activate(); },
                 markerObj => { markerObj.Deactivate(); },
                 markerObj => { Destroy(markerObj.gameObject); },
                 false, _defaultPoolSize, _maxPoolSize
                );
            }
        }

        public SelectionMarker GetMarker()
        {
            return _pool.Get();
        }

        public void ReturnMarker(SelectionMarker marker)
        {
            _pool.Release(marker);
        }

   
    }

}
