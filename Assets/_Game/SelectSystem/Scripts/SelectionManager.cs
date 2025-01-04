using System;
using System.Collections.Generic;
using UnityEngine;
using SelectionSystem.Marker;

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
                selectable.Select(_selectionMarkerPool.GetMarker() as SelectionMarker);
            }
        }

        public IReadOnlyList<ISelectable> GetSelectedObjects() => _selectedObjects;

        private void ExecuteCommand()
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if (_selectedObjects.Count > 0)
                {
                    foreach (var obj in _selectedObjects)
                    {
                        obj.Execute(hit.point);
                    }
                }
            }
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray, out RaycastHit hit))
            //{
            // //   ICommand command = DetermineCommand(hit.point);
            //    command?.Execute(hit.point);
            //}
        }

        //public interface ICommand
        //{
        //    public void Execute(Vector3 positon);
        //}
    }
}