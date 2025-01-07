using AStarPathfinding;
using DG.Tweening;
using ObjectPoolSystem;
using SelectionSystem;
using SelectionSystem.Marker;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Unit
{
    public abstract class UnitBase : MonoBehaviour, IAttacker, IPoolable
    {
        public UnityEvent onSelect;
        public UnityEvent onDeselect;
        public Action<int> OnHealthChange { get;  set; }
        public Transform unitVisual;
        public SpriteRenderer rankSpriteRenderer;
        public UnitData Data { get; private set; }

        protected StateManager _stateManager;

        private PoolSystem _poolSystem;
        private SelectionMarker _marker;
        private int _currentHealth;
        private UnitUIInfo _currentStrategy;
        [SerializeField] private float attackAnimForwardDistance = 0.25f;
        [SerializeField] private float attackAnimMoveTime = 0.2f;

      
        public PathfindingAgent Agent { get; private set; }
        public GameObject GameObject => gameObject;
        public GameObject Owner => gameObject;
        public Vector2 Objectsize => Vector2.one;

        private void Awake()
        {
            Agent = GetComponent<PathfindingAgent>();
        }

        public int CurrentHealth
        {
            get
            {
                return _currentHealth;
            }
            set
            {
                _currentHealth = value;
                OnHealthChange?.Invoke(_currentHealth);

            }
        }

        public void TakeDamage(int Damage)
        {
            CurrentHealth -= Damage;
            if (CurrentHealth <= 0)
            {
                KillUnit();
            }
        }
        public void KillUnit()
        {
            Agent.ResetOccupyNode();
            _poolSystem.Return(this);
        }

        #region Commands
        public void Select(SelectionMarker selectionMarker)
        {
            onSelect.Invoke();
            _marker = selectionMarker;
            _marker.AttachTo(transform, Vector3.zero, transform.localScale);

            _currentStrategy = new UnitUIInfo(this);
            ServiceLocator.Get<InfoController>().SelectEntity(_currentStrategy);
        }

        public void Deselect()
        {
            onDeselect.Invoke();
            if (_marker) _marker.Detach();


            ServiceLocator.Get<InfoController>().DeselectEntity(_currentStrategy);
        }

        public abstract void Command(Vector3 position);

        public abstract void Attack(IAttackable target);


        private void Update()
        {
            _stateManager?.currentState?.OnStateUpdate();
        }
        #endregion
        #region Pool Methods
        public void Init(PoolSystem poolSystem)
        {
            _poolSystem = poolSystem;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            gameObject.transform.position = Vector3.one * -100;
        }
        /// <summary>
        /// 0 - UnitData
        /// 1 - SpawnPosition
        /// </summary>
        /// <param name="args"></param>
        public void UpdateArgs(params object[] args)
        {
            Data = (UnitData)args[0];
            transform.position = (Vector3)args[1];
            CurrentHealth = Data.health;
            rankSpriteRenderer.sprite = Data.rankVisual;
            _stateManager = new StateManager(this, Data);

        }

        public void AnimateAttack(Action onAnimationComplete)
        {
            // 1) Grab current position for reference
            Vector3 originalPos = unitVisual.localPosition;

            // 2) Decide "forward" direction
            // In 2D, you might do "transform.up" or "transform.right"; in 3D, maybe "transform.forward"
            // For a simple 2D example, let's use "transform.up"
            Vector3 forwardPos = originalPos + (unitVisual.up * attackAnimForwardDistance);

            Sequence attackSeq = DOTween.Sequence();
            attackSeq.Append(
                unitVisual.DOLocalMove(forwardPos, attackAnimMoveTime).SetEase(Ease.OutQuad)
                );
            attackSeq.AppendCallback(() => onAnimationComplete?.Invoke());
            attackSeq.Append(
                unitVisual.DOLocalMove(originalPos, attackAnimMoveTime).SetEase(Ease.InQuad)
   );
        }

        #endregion
    }



}

