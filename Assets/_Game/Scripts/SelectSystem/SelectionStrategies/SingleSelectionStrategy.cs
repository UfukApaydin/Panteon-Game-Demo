using UnityEngine;
using UnityEngine.EventSystems;

namespace SelectionSystem
{
    public class SingleSelectionStrategy : ISelectionStrategy
    {
        public void Select(SelectionManager selectionManager)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            selectionManager.ClearSelection();

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
          
            
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<ISelectable>(out var selectable))
                {
                    selectionManager.AddToSelection(selectable);
                }
            }
        }
    }
}