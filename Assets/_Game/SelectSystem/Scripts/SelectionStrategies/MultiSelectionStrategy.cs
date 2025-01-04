using UnityEngine;

namespace SelectionSystem
{
    public class MultiSelectionStrategy : ISelectionStrategy
    {
        public void Select(SelectionManager selectionManager)
        {
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