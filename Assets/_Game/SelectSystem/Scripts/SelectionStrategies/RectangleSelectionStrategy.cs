using UnityEngine;

namespace SelectionSystem
{
    public class RectangleSelectionStrategy : ISelectionStrategy
    {
        private Camera _mainCamera;
        private RectangleDrawer _rectangleDrawer;
        private Vector3 _startPoint;
        public RectangleSelectionStrategy(RectangleDrawer rectangleDrawer)
        {
            _rectangleDrawer = rectangleDrawer;
            _mainCamera = Camera.main;
        }
        public void Select(SelectionManager selectionManager)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                _rectangleDrawer.gameObject.SetActive(true);
                _rectangleDrawer.StartDrawing(_startPoint);
            }

            if (Input.GetMouseButtonUp(0))
            {
                Vector3 endPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Rect selectionRect = new Rect(
                    Mathf.Min(_startPoint.x, endPoint.x),
                    Mathf.Min(_startPoint.y, endPoint.y),
                    Mathf.Abs(_startPoint.x - endPoint.x),
                    Mathf.Abs(_startPoint.y - endPoint.y)
                );

                Collider2D[] colliders = Physics2D.OverlapAreaAll(
                    new Vector2(selectionRect.xMin, selectionRect.yMin),
                    new Vector2(selectionRect.xMax, selectionRect.yMax)
                );

                foreach (var collider in colliders)
                {
                    ISelectable selectable = collider.GetComponent<ISelectable>();
                    if (selectable != null)
                    {
                        selectionManager.AddToSelection(selectable);
                    }
                }
            
                _rectangleDrawer.StopDrawing();
                _rectangleDrawer.gameObject.SetActive(false);
            }
        }
    }
}