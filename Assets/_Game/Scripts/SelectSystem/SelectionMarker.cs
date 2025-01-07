using UnityEngine;
using ObjectPoolSystem;

namespace SelectionSystem.Marker
{
    public class SelectionMarker : MonoBehaviour , IPoolable
    {
        private Transform _target; 
        private Vector3 _offset;
        private SpriteRenderer _spriteRenderer;

        public GameObject GameObject => gameObject;
        private PoolManager poolManager => ServiceLocator.Get<PoolManager>();
        public void Init()
        {
           
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public void AttachTo(Transform target , Vector3 offset, Vector2 size)
        {
            _target = target;
            _offset = offset;
            _spriteRenderer.size = size;
        }

        public void Detach()
        {
            _target = null;
            poolManager.ReturnObject(GameManager.Instance.gameData.markerType, this);
        
        }

        private void LateUpdate()
        {
            if (_target != null)
            {
                transform.position = _target.position + _offset;
            }
        }

        #region Pooling Methods

        public void Activate()
        {
            gameObject.SetActive(true);
            _spriteRenderer.size = Vector2.one;
        }
        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void UpdateArgs(params object[] args)
        {
            throw new System.NotImplementedException();
        }



        #endregion
    }

}
