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
               // Debug.Log("Pointer is over a UI element. Selection ignored.");
                return;
            }
            selectionManager.ClearSelection();

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
          
            Debug.Log($"raycast hit: {hit.collider.gameObject.name}");
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