using ObjectPoolSystem;
using SelectionSystem.Marker;
using System.Collections.Generic;
using UnityEngine;

namespace SelectionSystem
{
    public class SelectionManager : MonoBehaviour
    {
        private List<ISelectable> _selectedObjects = new();
        private ISelectionStrategy _selectionStrategy;

        private readonly ISelectionStrategy _singleSelection = new SingleSelectionStrategy();
        private readonly ISelectionStrategy _multiSelection = new MultiSelectionStrategy();

        private void Awake()
        {
            ServiceLocator.Register<SelectionManager>(this);
        }
        void Update()
        {
            ExecuteSelection();
        }
        private void ExecuteSelection()
        {
            if (Input.GetMouseButtonDown(0)) // Left-click
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    _selectionStrategy = _multiSelection;
                }
                else
                {
                    _selectionStrategy = _singleSelection;
                }
                _selectionStrategy.Select(this);
            }

            if (Input.GetMouseButtonDown(1)) // Right-click
            {
                ExecuteCommand();
            }

        }
        public void RemoveFromSelection(ISelectable selectable)
        {
            if (_selectedObjects.Contains(selectable))
            {
                _selectedObjects.Remove(selectable);
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
                var marker = ServiceLocator.Get<PoolManager>().GetObject(GameManager.Instance.gameData.markerType);
                selectable.Select(marker as SelectionMarker);
            }
        }

        public IReadOnlyList<ISelectable> GetSelectedObjects() => _selectedObjects;

        private void ExecuteCommand()
        {

            if (MouseTools.GetMousePosition(out RaycastHit2D hit))
            {
                if (hit.collider != null)
                {
                    ICommandStrategy command = hit.collider != null && hit.collider.GetComponent<ISelectable>() != null
                    ? new AttackCommand()
                    : new MoveCommand();

                    foreach (var obj in _selectedObjects)
                    {
                        command.Execute(obj, hit, hit.point);
                    }
                }
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


