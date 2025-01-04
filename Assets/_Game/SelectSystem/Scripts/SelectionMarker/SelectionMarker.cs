using UnityEngine;

namespace SelectionSystem.Marker
{
    public class SelectionMarker : MonoBehaviour
    {
        private Transform _target; 
        private Vector3 _offset;
        private SelectionMarkerPool _selectionMarkerPool;
        private SpriteRenderer _spriteRenderer;
        public void Construct(SelectionMarkerPool selectionMarkerPool)
        {
            _selectionMarkerPool = selectionMarkerPool;
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
            _selectionMarkerPool.ReturnMarker(this);
        
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

        #endregion
    }

}
