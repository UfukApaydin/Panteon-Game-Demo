using A_Pathfinding.Pathfinding;
using SelectionSystem;
using SelectionSystem.Marker;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Unit
{
    public abstract class UnitBase : MonoBehaviour, IAttacker
    {
        public UnityEvent onSelect;
        public UnityEvent onDeselect;

        public int currentHealth;

        private UnitData _unitData;
        private SelectionMarker _marker;

        public PathfindingAgent Agent { get; private set; }
        private void Awake()
        {
            Agent = GetComponent<PathfindingAgent>();
        }

        public void Init(UnitData unitData)
        {
            _unitData = unitData;
            currentHealth = _unitData.health;
        }
        float minDistance = .3f;
        float separationForceFactor = 1f;
        public Vector3 velocity = Vector3.zero;
        public LayerMask unitLayer;
        private void Update()
        {
            Vector3 separation = Vector3.zero;
           Collider2D[] overlapUnits = Physics2D.OverlapBoxAll(transform.position,Vector2.one,0, unitLayer);
            foreach (Collider2D other in overlapUnits)
            {
                if (other == this) continue;
                float dist = Mathf.Clamp( Vector3.Distance(transform.position, other.transform.position),0.1f,1);
                if (dist < minDistance)
                {
                    Vector3 dir = (transform.position - other.transform.position).normalized;
                    separation += dir / dist; // stronger if very close
                }
            }
            // Add separation to your normal path-following velocity
            velocity = /*pathFollowingVelocity +*/ separationForceFactor * separation;
            transform.position += velocity * Time.deltaTime;
        }
        public void Select(SelectionMarker selectionMarker)
        {
            onSelect.Invoke();
            _marker = selectionMarker;
            _marker.AttachTo(transform, Vector3.zero, transform.localScale);
        }

        public void Deselect()
        {
            onDeselect.Invoke();
            if (_marker) _marker.Detach();
        }

        public void Execute(Vector3 position)
        {
            Agent.MoveToPosition(position);
        }

        public void Attack(GameObject target)
        {
            Agent.MoveToPosition(target.transform.position);
            Debug.Log("ATTACK");
        }
 
    }
}

