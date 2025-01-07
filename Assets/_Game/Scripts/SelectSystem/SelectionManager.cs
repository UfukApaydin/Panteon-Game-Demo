using ObjectPoolSystem;
using SelectionSystem.Marker;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SelectionSystem
{
    public class SelectionManager : MonoBehaviour
    {
        private List<ISelectable> _selectedObjects = new();
        private ISelectionStrategy _selectionStrategy;

        [SerializeReference] private PoolSystem _selectionMarkerPool;
        [SerializeField] private RectangleDrawer _rectangleDrawer;

        private void Start()
        {
            InitPool();
        }
        void Update()
        {
            ExecuteSelection();
        }
        private void InitPool()
        {
            if (_selectionMarkerPool == null)
            {

                throw new InvalidOperationException("SelectionMarkerPool cannot be null");
            }
            else
            {
                _selectionMarkerPool.Init();
            }
        }
        private void ExecuteSelection()
        {
            if (Input.GetMouseButtonDown(0)) // Left-click
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    _selectionStrategy = new MultiSelectionStrategy();
                }
                else
                {
                    _selectionStrategy = new SingleSelectionStrategy();
                }
                _selectionStrategy.Select(this);
            }

            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftAlt)) // Drag box start
            {

                _selectionStrategy = new RectangleSelectionStrategy(_rectangleDrawer);
                //       _selectionStrategy = new RectangleSelectionStrategy();
                _selectionStrategy.Select(this);
            }

            if (Input.GetMouseButtonDown(1)) // Right-click
            {
                ExecuteCommand();
            }

        }
        public void ClearSelection()
        {
            foreach (var obj in _selectedObjects)
            {
                obj.Deselect();
            }
            _selectedObjects.Clear();
        }

        public void AddToSelection(ISelectable selectable)
        {
            if (!_selectedObjects.Contains(selectable))

            {
                _selectedObjects.Add(selectable);
                selectable.Select(_selectionMarkerPool.Get() as SelectionMarker);
            }
        }

        public IReadOnlyList<ISelectable> GetSelectedObjects() => _selectedObjects;

        private void ExecuteCommand()
        {

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            Vector3 targetPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            ICommandStrategy command = hit.collider != null && hit.collider.GetComponent<ISelectable>() != null
                ? new AttackCommand()
                : new MoveCommand();

            foreach (var obj in _selectedObjects)
            {
                command.Execute(obj, hit, targetPoint);
            }
        }
    }


    public interface ICommandStrategy
    {
        void Execute(ISelectable source, RaycastHit2D hit, Vector3 targetPoint);
    }

    public class AttackCommand : ICommandStrategy
    {
        public void Execute(ISelectable source, RaycastHit2D hit, Vector3 targetPoint)
        {
            if (hit.collider != null && hit.collider.TryGetComponent<IAttackable>(out var target))
            {
              
                if (source is IAttacker)
                    (source as IAttacker).Attack(target);
            }
            else
            {
                Debug.LogWarning("AttackCommand executed, but no valid target was found.");
            }
        }
    }

    public class MoveCommand : ICommandStrategy
    {
        public void Execute(ISelectable source, RaycastHit2D hit, Vector3 targetPoint)
        {
            source.Command(targetPoint);
        }
    }
}


