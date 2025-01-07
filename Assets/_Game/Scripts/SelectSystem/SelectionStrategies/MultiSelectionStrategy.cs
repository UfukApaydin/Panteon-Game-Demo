using UnityEngine;

namespace SelectionSystem
{
    public class MultiSelectionStrategy : ISelectionStrategy
    {
        public void Select(SelectionManager selectionManager)
        {

            if (MouseTools.GetMousePosition(out RaycastHit2D hit))
            {
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
}